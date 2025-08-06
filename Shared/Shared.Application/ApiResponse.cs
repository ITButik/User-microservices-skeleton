using System.Net;

namespace Shared.Application
{
    public class ApiResponse<T>
    {
        public T? Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsEmpty { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Exception? Exception { get; private set; }

        private ApiResponse() { }

        public static ApiResponse<T> Success(T data, HttpStatusCode statusCode) =>
            new() { Data = data, IsSuccess = true, StatusCode = statusCode };

        public static ApiResponse<T> Empty(HttpStatusCode statusCode) =>
            new() { IsSuccess = true, IsEmpty = true, StatusCode = statusCode };

        public static ApiResponse<T> Failure(HttpStatusCode statusCode, string? errorMessage) =>
            new() { IsSuccess = false, StatusCode = statusCode, ErrorMessage = errorMessage };

        public static ApiResponse<T> ExceptionResponse(Exception ex) =>
            new() { IsSuccess = false, Exception = ex, StatusCode = HttpStatusCode.InternalServerError, ErrorMessage = ex.Message };
    }

}
