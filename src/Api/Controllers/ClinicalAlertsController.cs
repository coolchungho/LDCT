using LDCT.Api.Security;
using LDCT.Api.Services;
using LDCT.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDCT.Api.Controllers;

[ApiController]
[Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Physician},{AppRoles.Administrator}")]
[Route("api/v1/[controller]")]
public class ClinicalAlertsController(LdctDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> List(CancellationToken ct)
    {
        var alerts = await ClinicalAlertEvaluator.EvaluateAsync(db, ct);
        return Ok(alerts);
    }
}
