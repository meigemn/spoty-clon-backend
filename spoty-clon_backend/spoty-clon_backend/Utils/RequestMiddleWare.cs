using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Net;

namespace spoty_clon_backend.Utils
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBlockingService _blockingService;

        public RequestMiddleware(RequestDelegate next, IBlockingService blockingService)
        {
            _next = next;
            _blockingService = blockingService;
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            var isBlocked = _blockingService.IsBlocked(remoteIp);
            if (!isBlocked)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            var language = context.Request.Headers.FirstOrDefault(f => f.Key.Equals("Language"));

            SetCulture(language);

            await _next.Invoke(context);
        }

        private void SetCulture(KeyValuePair<string, StringValues> language)
        {
            var culture = "en-US";
            if (language.Value.Any())
            {
                switch (language.Value.FirstOrDefault())
                {
                    case "2":
                    case "es-ES":
                        culture = "es-ES";
                        break;
                    default:
                        culture = "en-US";
                        break;
                }
            }

            CultureInfo cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
