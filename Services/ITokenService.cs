using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MegaLivros.Services;

public interface ITokenService
{
  public  JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config);

  public string GenerateRefreshToken();

   public ClaimsPrincipal GetPrincipalFromExpiredToekn(string token, IConfiguration _config);
}
