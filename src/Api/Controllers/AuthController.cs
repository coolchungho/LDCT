using LDCT.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LDCT.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    IOptions<DevAuthUsersOptions> usersOptions,
    JwtTokenIssuer tokenIssuer) : ControllerBase
{
    public record LoginRequest(string UserName, string Password);

    public record LoginResponse(string Token, string UserId, string Role, string UserName, DateTimeOffset ExpiresAtUtc);

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest body)
    {
        var user = usersOptions.Value.Users.FirstOrDefault(u =>
            string.Equals(u.UserName, body.UserName, StringComparison.OrdinalIgnoreCase)
            && u.Password == body.Password);
        if (user == null)
            return Unauthorized();

        var token = tokenIssuer.CreateToken(user.UserId, user.UserName, user.Role);
        var exp = DateTimeOffset.UtcNow.AddHours(8);
        return Ok(new LoginResponse(token, user.UserId, user.Role, user.UserName, exp));
    }
}
