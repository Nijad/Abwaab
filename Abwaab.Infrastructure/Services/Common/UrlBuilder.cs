using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace Abwaab.Infrastructure.Services.Common
{
    public class UrlBuilder : IUrlBuilder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _configuration;

        public UrlBuilder(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _configuration = configuration;
        }

        public string GetCancelEmailChangeUrl() => BuildUrl(Constants.CANCEL_EMAIL_CHANGE_ACTION, Constants.AUTH_CONTROLLER);

        public string GetCancelPhoneChangeUrl() => BuildUrl(Constants.CANCEL_PHONE_CHANGE_ACTION, Constants.AUTH_CONTROLLER);


        private string BuildUrl(string action, string controller)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // If we have an active HttpContext, generate a fully qualified URL
            if (httpContext != null)
            {
                return _linkGenerator.GetUriByAction(
                    httpContext,
                    action: action,
                    controller: controller,
                    values: null,
                    scheme: httpContext.Request.Scheme,
                    host: httpContext.Request.Host,
                    pathBase: httpContext.Request.PathBase
                );
            }

            // Fallback: use configured BaseUrl (for background jobs, console apps, etc.)
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("BaseUrl is not configured and HttpContext is not available.");

            return $"{baseUrl.TrimEnd('/')}/api/{controller}/{action}";
        }
    }
}
