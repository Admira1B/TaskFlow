using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Identity.Domain.Options;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Services;

namespace TaskFlow.Identity.Application.Services {
    public class JsonWebTokenService(IOptions<JsonWebTokenGenerationOptions> options, UserManager<User> userManager) : IJsonWebTokenService {
        private readonly JsonWebTokenGenerationOptions _options = options.Value;
        private readonly UserManager<User> _userManager = userManager;
        public async Task<string> GenerateWebTokenAsync(User user) {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles) {
                claims.Add(new(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var tokenDescriptor = new SecurityTokenDescriptor { 
                Issuer = _options.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_options.ExpiresHours),
                SigningCredentials = credentials
            };

            if (_options.ValidAudiences.Length > 0) {
                tokenDescriptor.Audience = null;
                tokenDescriptor.Claims ??= new Dictionary<string, object>();
                tokenDescriptor.Claims["aud"] = _options.ValidAudiences;
            }

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
