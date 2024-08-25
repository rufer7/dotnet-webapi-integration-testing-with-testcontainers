using ArbitraryApp.Domain;
using ArbitraryApp.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArbitraryApp.Server.Controllers;

[ValidateAntiForgeryToken]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class ArbitraryController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ArbitraryController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IEnumerable<string>> Get()
    {
        var records = await _dbContext.ArbitraryRecords.ToListAsync();

        return records.Select(r => r.Value);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AssignmentToAdminRoleRequired)]
    public async Task Post([FromBody] ArbitraryRecordRequestModel request)
    {
        var record = new ArbitraryRecord
        {
            Name = request.Name,
            Value = request.Value
        };

        _dbContext.ArbitraryRecords.Add(record);
        await _dbContext.SaveChangesAsync();
    }
}
