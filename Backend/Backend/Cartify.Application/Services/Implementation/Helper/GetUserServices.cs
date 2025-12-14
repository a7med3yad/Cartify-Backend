using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cartify.Application.Services.Implementation.Helper
{
    public class GetUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetMerchantIdFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            var userClaims = httpContext.User;
            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
                return null;

            var merchantId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? userClaims.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return merchantId;
        }
        public string? GetUserNameFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            var userClaims = httpContext.User;
            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
                return null;

            var username = userClaims.FindFirst(ClaimTypes.Name)?.Value
                           ?? userClaims.FindFirst(JwtRegisteredClaimNames.Email)?.Value
                           ?? userClaims.Identity?.Name;

            return username;
        }
    }
}
