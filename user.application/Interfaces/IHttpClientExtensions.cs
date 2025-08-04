using user.sharedkernel.Utilities;

namespace user.application.Interfaces;

public interface IHttpClientExtensions
{
    Task<ApiResponse<T>> GetAsync<T>(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<ApiResponse<T>> PostAsync<T>(string url, object payload, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
}
