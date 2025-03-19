using Microsoft.AspNetCore.Mvc;
using UserMenagerApi.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    public AuthController(IGenerateTokenService generateTokenService)
    {
        _generateTokenService = generateTokenService;
    }

    private readonly IGenerateTokenService _generateTokenService;
    [HttpPost("mock-login")]
    public IActionResult MockLogin()
    {
        var token = _generateTokenService.GenerateToken();
        return Ok(new { access_token = token });
    }
}