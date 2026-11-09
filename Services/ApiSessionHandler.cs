using System.Net;

namespace RaceDay.MVC.Services
{
    public class ApiSessionHandler : DelegatingHandler
    {
        private const string ApiSessionCookieName = "RaceMgmtSession"; // Matches API cookie name
        private readonly IHttpContextAccessor _accessor;

        public ApiSessionHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var session = _accessor.HttpContext?.Session;
            var storedCookie = session?.GetString(SessionKeys.ApiCookie);
            if (!string.IsNullOrEmpty(storedCookie))
            {
                request.Headers.TryAddWithoutValidation("Cookie", storedCookie);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (session != null)
            {
                if (response.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders))
                {
                    foreach (var header in setCookieHeaders)
                    {
                        var nameAndValue = header.Split(';')[0].Trim();
                        if (nameAndValue.StartsWith(ApiSessionCookieName + "=", StringComparison.Ordinal))
                        {
                            session.SetString(SessionKeys.ApiCookie, nameAndValue);
                        }
                    }
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    session.Clear();
                }
            }

            return response;
        }
    }
}