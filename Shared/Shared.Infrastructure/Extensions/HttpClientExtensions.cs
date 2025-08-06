using Microsoft.Extensions.Logging;
using Shared.Application;
using Shared.Application.Extensions;
using System.Text;
using System.Text.Json;

namespace Shared.Infrastructure.Extensions
{
    public class HttpClientExtensions : IHttpClientExtensions
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientExtensions> _logger;

        public HttpClientExtensions(HttpClient httpClient, ILogger<HttpClientExtensions> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateUrl(url);

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                return await HandleResponse<T>(response, url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET request failed for URL: {Url}", url);
                return ApiResponse<T>.ExceptionResponse(ex);
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string url, object payload, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateUrl(url);

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = SerializePayload(payload)
                };
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                return await HandleResponse<T>(response, url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST request failed for URL: {Url}", url);
                return ApiResponse<T>.ExceptionResponse(ex);
            }
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string url, object payload, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateUrl(url);

                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = SerializePayload(payload)
                };
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                return await HandleResponse<T>(response, url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PUT request failed for URL: {Url}", url);
                return ApiResponse<T>.ExceptionResponse(ex);
            }
        }

        public async Task<ApiResponse<string>> GetRawResponseAsync(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateUrl(url);

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                return await HandleRawResponse(response, url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET request failed for URL: {Url}", url);
                return ApiResponse<string>.ExceptionResponse(ex);
            }
        }

        public async Task<ApiResponse<string>> PostRawResponseAsync(string url, string payload, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateUrl(url);

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                return await HandleRawResponse(response, url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST request failed for URL: {Url}", url);
                return ApiResponse<string>.ExceptionResponse(ex);
            }
        }

        public async Task<ApiResponse<string>> PutRawResponseAsync(string url, string payload, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateUrl(url);

                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                return await HandleRawResponse(response, url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PUT request failed for URL: {Url}", url);
                return ApiResponse<string>.ExceptionResponse(ex);
            }
        }

        // Private helper methods
        private void ValidateUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }
        }

        private void AddHeaders(HttpRequestMessage request, IDictionary<string, string>? headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        private StringContent SerializePayload(object payload)
        {
            var json = JsonSerializer.Serialize(payload);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response, string url, CancellationToken cancellationToken)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("{Method} failed: {StatusCode} - {Content}", response.RequestMessage?.Method, response.StatusCode, content);
                return ApiResponse<T>.Failure(response.StatusCode, content);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogInformation("{Method} succeeded but response was empty for URL: {Url}", response.RequestMessage?.Method, url);
                return ApiResponse<T>.Empty(response.StatusCode);
            }

            try
            {
                var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result == null)
                {
                    _logger.LogWarning("Deserialization returned null for URL: {Url}", url);
                    return ApiResponse<T>.Empty(response.StatusCode);
                }
                return ApiResponse<T>.Success(result, response.StatusCode);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response for URL: {Url}", url);
                return ApiResponse<T>.Failure(response.StatusCode, "Deserialization error.");
            }
        }

        private async Task<ApiResponse<string>> HandleRawResponse(HttpResponseMessage response, string url, CancellationToken cancellationToken)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("{Method} failed: {StatusCode} - {Content}", response.RequestMessage?.Method, response.StatusCode, content);
                return ApiResponse<string>.Failure(response.StatusCode, content);
            }

            return ApiResponse<string>.Success(content, response.StatusCode);
        }
    }

}
