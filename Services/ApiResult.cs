namespace RaceDay.MVC.Services
{
    public class ApiResult<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public int StatusCode { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static ApiResult<T> Ok(T? data, int statusCode = 200) =>
            new() { Success = true, Data = data, StatusCode = statusCode };

        public static ApiResult<T> Fail(int statusCode, string message) =>
            new() { Success = false, StatusCode = statusCode, ErrorMessage = message };
    }
}