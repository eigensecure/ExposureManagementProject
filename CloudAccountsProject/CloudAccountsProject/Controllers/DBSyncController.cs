using Azure.Storage.Blobs;
using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.DbSyncModels;
using CloudAccountsShared.DbSyncModels.CloudMasterSync;
using CloudAccountsShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudAccountsProject.Controllers;

public class DBSyncController(CloudAccountsDbContext dbcontext, IConfiguration config) : BaseApiController
{
    private readonly CloudAccountsDbContext _Dbcontext = dbcontext;
    private readonly IConfiguration _config = config;

    private const int BatchSize = 5000;
    int totalCount = 0;

    [HttpPost("cloudAccountsMaster")]
    public async Task<IActionResult> CloudMasterSync([FromBody] BlobRequest request)
    {
        var toInsert = new List<CloudAccountsMaster>();
        var toUpdate = new List<CloudAccountsMaster>();

        var insertSet = new HashSet<string>();
        var updateSet = new HashSet<string>();

        int insertCount = 0;
        int updateCount = 0;
        totalCount = 0;

        try
        {
            var activeContainer = _config["AzureContainer:Active"];
            var _connectionString = _config[$"AzureContainer:{activeContainer}:ConnectionString"];

            if (request == null || string.IsNullOrEmpty(request.BlobName))
                return BadRequest(new { error = "Invalid blob information." });

            //_logger.Information($"### [{DateTime.UtcNow}]  ");
            //_logger.Information("Message Received, Fetching file from the container..  ");
            ////Console.WriteLine(request.Container);
            ////Console.WriteLine(request.BlobName);
            //_logger.Information("```json\n" + new { request.BlobName, request.Container } + "\n```\n  ");

            var blobClient = new BlobClient(_connectionString, request.Container, $"CloudMasterSync/Data/{request.BlobName}");

            if (!await blobClient.ExistsAsync())
                return NotFound(new { error = "Blob not found." });

            await using var blobStream = await blobClient.OpenReadAsync();

            using var streamReader = new StreamReader(blobStream);
            using var jsonReader = new JsonTextReader(streamReader);

            const int chunkSize = 1000;
            var chunk = new List<ActionFormat>(chunkSize);

            bool insideArray = false;

            while (await jsonReader.ReadAsync())
            {
                if (jsonReader.TokenType == JsonToken.StartArray)
                {
                    insideArray = true;
                    continue;
                }

                if (insideArray && jsonReader.TokenType == JsonToken.StartObject)
                {
                    try
                    {
                        var token = JToken.Load(jsonReader);
                        var record = token.ToObject<ActionFormat>();

                        if (record == null || string.IsNullOrEmpty(record.CloudAccountId))
                            continue;

                        chunk.Add(record);
                    }
                    catch (JsonException)
                    {
                        jsonReader.Skip();
                    }

                    if (chunk.Count == chunkSize)
                    {
                        await ProcessChunkAsync(chunk, toInsert, toUpdate);
                        chunk.Clear();
                    }
                }

                if (insideArray && jsonReader.TokenType == JsonToken.EndArray)
                {
                    break;
                }
            }

            if (chunk.Count > 0)
            {
                await ProcessChunkAsync(chunk, toInsert, toUpdate);
            }

            if (toInsert.Count > 0 || toUpdate.Count > 0)
            {
                await SaveBatchAsync(toInsert, toUpdate);
                toInsert.Clear();
                toUpdate.Clear();
                insertSet.Clear();
                updateSet.Clear();
            }

            return Ok(new
            {
                message = "Processing completed",
                inserted = insertCount,
                updated = updateCount
            });
        }
        catch (Exception ex)
        {
            //_logger.Error(ex, "Error while processing streamed JSON  ");
            return StatusCode(500, ex.Message);
        }

        async Task ProcessChunkAsync(List<ActionFormat> chunk, List<CloudAccountsMaster> toInsert, List<CloudAccountsMaster> toUpdate)
        {
            var ids = chunk
                .Where(x => x.Action.Equals("Update", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.CloudAccountId)
                .Distinct()
                .ToList();

            var existingDict = await _Dbcontext.CloudAccountsMasters
                .Where(x => ids.Contains(x.CloudAccountId))
                .ToDictionaryAsync(x => x.CloudAccountId);

            foreach (var record in chunk)
            {
                if (record.NewValue == null)
                    continue;
                if (record.Action.Equals("Insert", StringComparison.OrdinalIgnoreCase))
                {
                    var newItem = new CloudAccountsMaster
                    {
                        Provider = record.NewValue.Provider,
                        CloudAccountId = record.NewValue.CloudAccountId,
                        CloudName = record.NewValue.CloudName,
                        CloudOrgId = record.NewValue.CloudOrgId,
                        CloudRootAccountId = record.NewValue.CloudRootAccountId,
                        RegistrationType = record.NewValue.RegistrationType,
                        DeploymentMethod = record.NewValue.DeploymentMethod,
                        RegisteredAtCrwd = record.NewValue.RegisteredAtCrwd,
                        LastUpdatedAtCrwd = record.NewValue.LastUpdatedAtCrwd,
                        Iomstatus = record.NewValue.Iomstatus,
                        RealTimeVisibilityAndDetectionStatus = record.NewValue.RealTimeVisibilityAndDetectionStatus,
                        OneClickSensorStatus = record.NewValue.OneClickSensorStatus,
                        IdentityProtectionStatus = record.NewValue.IdentityProtectionStatus,
                        Dspmstatus = record.NewValue.Dspmstatus,
                        VulnerabilityScanningStatus = record.NewValue.VulnerabilityScanningStatus,
                        RawJson = record.NewValue.RawJson,
                        IsActive = true,
                        DateCreated = DateTime.UtcNow,
                        DateModified = DateTime.UtcNow
                    };

                    if (insertSet.Add(newItem.CloudAccountId))
                    {
                        toInsert.Add(newItem);
                        insertCount++;
                    }
                }
                else if (record.Action.Equals("Update", StringComparison.OrdinalIgnoreCase))
                {
                    if (!existingDict.TryGetValue(record.CloudAccountId, out var existing))
                        continue;

                    var incoming = record.NewValue;

                    if (!string.IsNullOrEmpty(incoming.Provider))
                        existing.Provider = incoming.Provider;

                    if (!string.IsNullOrEmpty(incoming.CloudName))
                        existing.CloudName = incoming.CloudName;

                    if (!string.IsNullOrEmpty(incoming.CloudOrgId))
                        existing.CloudOrgId = incoming.CloudOrgId;

                    if (!string.IsNullOrEmpty(incoming.CloudRootAccountId))
                        existing.CloudRootAccountId = incoming.CloudRootAccountId;

                    if (!string.IsNullOrEmpty(incoming.RegistrationType))
                        existing.RegistrationType = incoming.RegistrationType;

                    if (!string.IsNullOrEmpty(incoming.DeploymentMethod))
                        existing.DeploymentMethod = incoming.DeploymentMethod;

                    if (incoming.RegisteredAtCrwd != null)
                        existing.RegisteredAtCrwd = incoming.RegisteredAtCrwd;

                    if (incoming.LastUpdatedAtCrwd != null)
                        existing.LastUpdatedAtCrwd = incoming.LastUpdatedAtCrwd;

                    if (!string.IsNullOrEmpty(incoming.Iomstatus))
                        existing.Iomstatus = incoming.Iomstatus;

                    if (!string.IsNullOrEmpty(incoming.RealTimeVisibilityAndDetectionStatus))
                        existing.RealTimeVisibilityAndDetectionStatus = incoming.RealTimeVisibilityAndDetectionStatus;

                    if (!string.IsNullOrEmpty(incoming.OneClickSensorStatus))
                        existing.OneClickSensorStatus = incoming.OneClickSensorStatus;

                    if (!string.IsNullOrEmpty(incoming.IdentityProtectionStatus))
                        existing.IdentityProtectionStatus = incoming.IdentityProtectionStatus;

                    if (!string.IsNullOrEmpty(incoming.Dspmstatus))
                        existing.Dspmstatus = incoming.Dspmstatus;

                    if (!string.IsNullOrEmpty(incoming.VulnerabilityScanningStatus))
                        existing.VulnerabilityScanningStatus = incoming.VulnerabilityScanningStatus;

                    if (!string.IsNullOrEmpty(incoming.RawJson))
                        existing.RawJson = incoming.RawJson;

                    if (incoming.IsActive.HasValue)
                        existing.IsActive = incoming.IsActive;

                    existing.DateModified = DateTime.UtcNow;

                    if (updateSet.Add(existing.CloudAccountId))
                    {
                        toUpdate.Add(existing);
                        updateCount++;
                    }
                }

                if (toInsert.Count + toUpdate.Count >= BatchSize)
                {
                    await SaveBatchAsync(toInsert, toUpdate);
                    toInsert.Clear();
                    toUpdate.Clear();
                    insertSet.Clear();
                    updateSet.Clear();
                }
            }
        }
    }

    private async Task SaveBatchAsync(List<CloudAccountsMaster> toInsert, List<CloudAccountsMaster> toUpdate)
    {
        if (toInsert.Count > 0)
        {
            await _Dbcontext.CloudAccountsMasters.AddRangeAsync(toInsert);
        }

        _Dbcontext.ChangeTracker.DetectChanges();

        await _Dbcontext.SaveChangesAsync();
        _Dbcontext.ChangeTracker.Clear();
    }
}
