using Abwaab.Application.Common.Constants;
using Abwaab.Application.Interfaces;
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

        public string GetCancelEmailChangeUrl(string changingCode) => BuildUrl(GeneralConstants.CANCEL_EMAIL_CHANGE_ACTION, GeneralConstants.AUTH_CONTROLLER, changingCode);

        public string GetCancelPhoneChangeUrl(string changingCode) => BuildUrl(GeneralConstants.CANCEL_PHONE_CHANGE_ACTION, GeneralConstants.AUTH_CONTROLLER, changingCode);


        private string BuildUrl(string action, string controller, string changingCode)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // If we have an active HttpContext, generate a fully qualified URL
            if (httpContext != null)
            {
                return _linkGenerator.GetUriByAction(
                    httpContext,
                    action: action,
                    controller: controller,
                    values: new { changingCode = changingCode },
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
