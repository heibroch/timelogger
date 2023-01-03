using Microsoft.Extensions.DependencyInjection;
using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.MessageBus
{
    public static class DependencyInjection
    {
        public static void AddInternalMessageBus(this IServiceCollection serviceCollection) => serviceCollection.AddSingleton<IInternalMessageBus, InternalMessageBus>();
    }
}
