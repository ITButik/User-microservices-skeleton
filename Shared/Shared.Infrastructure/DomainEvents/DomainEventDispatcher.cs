using Microsoft.Extensions.DependencyInjection;
using Shared.Domain;
using System.Collections.Concurrent;

namespace Shared.Infrastructure.DomainEvents;

internal sealed class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            Type domainEventType = domainEvent.GetType();
            Type handlerType = HandlerTypeDictionary.GetOrAdd(domainEventType, et => typeof(IDomainEventHandler<>).MakeGenericType(et));

            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null)
                {
                    continue;
                }

                var handlerwrapper = HandlerWrapper.Create(handler, domainEventType);
                await handlerwrapper.HandleAsync(domainEvent, cancellationToken);
            }
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);

        public static HandlerWrapper Create(object handler, Type domainEventType)
        {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(domainEventType, ht => typeof(HandlerWrapper<>).MakeGenericType(ht));
            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler);
        }
    }

    private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper
        where T : IDomainEvent
    {
        private readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;
        public override async Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await _handler.HandleAsync(domainEvent, cancellationToken);
        }
    }
}