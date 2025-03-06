using IdentityAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAPI.Services
{
    public interface IAuthorizationService
    {
        public Task<JwtSecurityToken> GetLoginToken(LoginModel loginModel);
    }
}
