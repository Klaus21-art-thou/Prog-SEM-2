using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class RaceDayApiClient
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public RaceDayApiClient(HttpClient http)
        {
            _http = http;
        }

        public Task<ApiResult<T>> GetAsync<T>(string url) => SendAsync<T>(HttpMethod.Get, url, null);

        public Task<ApiResult<T>> PostAsync<T>(string url, object body) => SendAsync<T>(HttpMethod.Post, url, body);

        public Task<ApiResult<T>> PutAsync<T>(string url, object body) => SendAsync<T>(HttpMethod.Put, url, body);

        public async Task<ApiResult<bool>> DeleteAsync(string url)
        {
            var result = await SendAsync<object>(HttpMethod.Delete, url, null);
            return result.Success
                ? ApiResult<bool>.Ok(true, result.StatusCode)
                : ApiResult<bool>.Fail(result.StatusCode, result.ErrorMessage ?? "Something went wrong.");
        }

        private async Task<ApiResult<T>> SendAsync<T>(HttpMethod method, string url, object? body)
        {
            try
            {
                using var request = new HttpRequestMessage(method, url);
                if (body != null)
                {
                    request.Content = JsonContent.Create(body, body.GetType(), options: JsonOptions);
                }

                using var response = await _http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var message = await ReadErrorMessageAsync(response);
                    return ApiResult<T>.Fail((int)response.StatusCode, message);
                }

                if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0)
                {
                    return ApiResult<T>.Ok(default, (int)response.StatusCode);
                }

                var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
                return ApiResult<T>.Ok(data, (int)response.StatusCode);
            }
            catch (HttpRequestException)
            {
                return ApiResult<T>.Fail(503, "The RaceDay service is not reachable right now. Please try again shortly.");
            }
            catch (TaskCanceledException)
            {
                return ApiResult<T>.Fail(504, "The RaceDay service took too long to respond. Please try again.");
            }
            catch (JsonException)
            {
                return ApiResult<T>.Fail(500, "We received an unexpected reply from the RaceDay service.");
            }
        }

        private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
        {
            try
            {
                var text = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    using var doc = JsonDocument.Parse(text);
                    if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                        doc.RootElement.TryGetProperty("message", out var message) &&
                        message.ValueKind == JsonValueKind.String)
                    {
                        return message.GetString() ?? "An error occurred.";
                    }
                    return text; // Return plain error string if API returned string response
                }
            }
            catch (JsonException) { }
            return "Something went wrong. Please try again.";
        }
    }
}