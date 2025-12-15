using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace spoty_clon_backend.Utils
{
    public interface IBlockingService
    {
        bool IsBlocked(IPAddress ipAddress);
        bool IsAuthorize(HttpContext context, string permissionRead, string permissionWrite);
    }
    public sealed class BlockingService : IBlockingService
    {
        private readonly List<string> _blockedIps;

        public BlockingService(IConfiguration configuration)
        {
            var blockedIps = configuration.GetValue<string>("BlockedIps");
            if (blockedIps == null)
            {
                _blockedIps = new List<string>();
            }
            else
            {
                _blockedIps = blockedIps.Split(',').ToList();
            }
        }

        public bool IsBlocked(IPAddress ipAddress) => _blockedIps.Contains(ipAddress.ToString());

        public bool IsAuthorize(HttpContext context, string permissionRead, string permissionWrite)
        {
            var authorization = context.Request.Headers.FirstOrDefault(f => f.Key.Equals("Authorization"));
            if (authorization.Value.Any())
            {
                var token = authorization.Value.FirstOrDefault();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var method = context.Request.Method;
                if (method.Equals("POST"))
                {
                    var path = context.Request.Path.Value.ToUpper();
                    if (!string.IsNullOrEmpty(path) && path.Contains("/GET"))
                    {
                        method = "GET";
                    }
                }
                if (method.Equals("GET"))
                {
                    if (!jwtSecurityToken.Claims.Any(claim => claim.Type == permissionRead))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!jwtSecurityToken.Claims.Any(claim => claim.Type == permissionWrite))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
