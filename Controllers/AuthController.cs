using MegaLivros.DTO;
using MegaLivros.Models;
using MegaLivros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MegaLivros.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService,
        UserManager<AplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
    {
        var user = await _userManager.FindByNameAsync(model.Username!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("id", user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles) {
            
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAcessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

            user.RefereshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes); 

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo

            });
        }
        return Unauthorized();
    }
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "UserAlready exists" });
        }

        AplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
        };
        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed" });
        }

        return Ok(new ResponseDTO { Status = "Sucess", Message = "User created successfully" });
    }
    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]

    public async Task<IActionResult> RefreshToken(TokenModelDTO tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid cliente request");
        }

        string? acessToken = tokenModel.AcessToken ?? throw new ArgumentNullException(nameof(tokenModel));

        string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException( nameof(tokenModel));

        var principal = _tokenService.GetPrincipalFromExpiredToekn(acessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Invalid acess toekn/refresh token");
        }

        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefereshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid acess token/refresh token");
        }

        var newAcessToken = _tokenService.GenerateAcessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
            refreshToken = newRefreshToken
        }) ;
    }
    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return BadRequest("Invalid user name");
        }

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    [HttpPost]
    [Route("CreateRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if(roleResult.Succeeded)
            {
                _logger.LogInformation(1, "Roles Added");
                return StatusCode(StatusCodes.Status200OK,
                    new ResponseDTO { Status = "Success", Message = $"Role{roleName} added successfully" });
            }
            else
            {
                _logger.LogInformation(2, "Error");
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO
                { Status = "Error", Message = $"Issue adding the new {roleName} role" });
            }
        }
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = "Role já existe" });
    }
    [HttpPost]
    [Route("AddUserToRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                return StatusCode(StatusCodes.Status200OK,
                new ResponseDTO { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" });
            }
            else
            {
                _logger.LogInformation(1, $"Error: Unable to add user{user.Email} to the {roleName} role");

                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Error: Unable to add user {user.Email} to the {roleName} role" });
            }
        }
        return BadRequest(new { error = "Unable to find user" });
      
    }
}
