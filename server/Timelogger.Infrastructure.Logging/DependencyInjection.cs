using Microsoft.Extensions.DependencyInjection;
using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.Logging
{
    public static class DependencyInjection
    {
        public static void AddInternalLogging(this IServiceCollection serviceCollection) => serviceCollection.AddSingleton<IInternalLogger, InternalLogger>();
    }
}
