using API.Helpers;
using System.Text;
using Core.Common;
using Asp.Versioning;
using Core.Entities.Business;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Options;

namespace API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly AppSettings _appSettings;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService,
        IConfiguration configuration,
        IOptions<AppSettings> appSettings)
    {
        _logger = logger;
        _authService = authService;
        _configuration = configuration;
        _appSettings = appSettings.Value;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _authService.Login(model.UserName, model.Password);
                if (result.Success)
                {
                    var token = GenerateJwtToken(result);
                    return Ok(new ResponseViewModel<AuthResultViewModel>
                    {
                        Success = true,
                        Data = token,
                        Message = "Login successful"
                    });
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while login");
                string message = $"An error occurred while login- " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "LOGIN_ERROR",
                        Message = message
                    }
                });
            }
        }

        return BadRequest(new ResponseViewModel
        {
            Success = false,
            Message = "Invalid input",
            Error = new ErrorViewModel
            {
                Code = "INPUT_VALIDATION_ERROR",
                Message = ModelStateHelper.GetErrors(ModelState)
            }
        });
    }

    [HttpPost, Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return Ok();
    }

    private AuthResultViewModel GenerateJwtToken(ResponseViewModel<UserViewModel> auth)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtConfig.Secret);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Aud, _appSettings.JwtConfig.ValidAudience),
                new Claim(JwtRegisteredClaimNames.Iss, _appSettings.JwtConfig.ValidIssuer),
                new Claim(JwtRegisteredClaimNames.Sub, auth.Data.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_appSettings.JwtConfig.TokenExpirationMinutes)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        return new AuthResultViewModel()
        {
            AccessToken = jwtToken,
            Success = true,
        };
    }
}