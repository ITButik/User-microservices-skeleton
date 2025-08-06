namespace Shared.Application.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException($"Handler not found for {request.GetType()}");

        return await handler.Handle((dynamic)request, cancellationToken);
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType());
        dynamic handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException($"Handler not found for {request.GetType()}");

        await handler.Handle((dynamic)request, cancellationToken);
    }
}
