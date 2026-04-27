using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudAccountsProject.Controllers;

public class LoginController(CloudAccountsDbContext Dbcontext) : BaseApiController
{
    private readonly IConfiguration _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

    private readonly CloudAccountsDbContext _context = Dbcontext;

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginTable model)
    {
        var user = await _context.UserLoginTables
            .FirstOrDefaultAsync(x => x.Username == model.Username);

        if (user == null || !VerifyPassword(model.Pass, user.Pass))
            return Unauthorized();

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    private bool VerifyPassword(string inputPassword, string storedPassword)
    {
        return inputPassword == storedPassword;
    }
}