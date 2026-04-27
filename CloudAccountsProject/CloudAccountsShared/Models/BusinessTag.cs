//using System;
//using System.Collections.Generic;
//using System.Text.Json.Serialization;

//namespace CloudAccountsShared.Models;

//public partial class BusinessTag
//{
//    public int Id { get; set; }

//    public int BusinessFunctionId { get; set; }

//    public string? TagName { get; set; }

//    public string? TagValue { get; set; }

//    public DateTime DateCreated { get; set; }

//    public DateTime DateModified { get; set; }

//    [JsonIgnore]
//    public virtual BusinessFunctionMaster? BusinessFunction { get; set; }
//}