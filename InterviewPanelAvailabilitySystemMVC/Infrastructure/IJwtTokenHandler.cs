using System.IdentityModel.Tokens.Jwt;

namespace InterviewPanelAvailabilitySystemMVC.Infrastructure
{
    public interface IJwtTokenHandler
    {
        JwtSecurityToken ReadJwtToken(string token);
    }
}
