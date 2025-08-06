using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;

namespace user.infrastructure.Exceptions
{
    public static class InfrastructureExceptionHandler
    {
        public static void Handle(Exception ex)
        {
            switch (ex)
            {
                // EF Core / SQL
                case DbUpdateConcurrencyException concurrencyEx:
                    throw new ExternalServiceException("Concurrency conflict in database operation", concurrencyEx);

                case DbUpdateException dbUpdateEx when dbUpdateEx.InnerException is SqlException sqlEx:
                    throw new ExternalServiceException($"SQL error {sqlEx.Number}: {sqlEx.Message}", sqlEx);

                case DbUpdateException dbUpdateEx:
                    throw new ExternalServiceException("Database update failed", dbUpdateEx);

                // HTTP / REST APIs
                case HttpRequestException httpEx:
                    throw new ExternalServiceException("HTTP request to external service failed", httpEx);

                case TaskCanceledException timeoutEx:
                    throw new ExternalServiceException("External service call timed out", timeoutEx);

                // File I/O
                case IOException ioEx:
                    throw new ExternalServiceException("File or stream operation failed", ioEx);

                // Threading / Parallelism
                case OperationCanceledException cancelEx:
                    throw new ExternalServiceException("Operation was cancelled", cancelEx);

                //// Messaging / Service Bus
                //case MessagingException msgEx:
                //    throw new ExternalServiceException("Message broker operation failed", msgEx);

                // Fallback
                default:
                    throw new ExternalServiceException("Unhandled infrastructure exception", ex);
            }
        }
    }

}
