using Microsoft.Extensions.DependencyInjection;

namespace Timelogger.Infrastructure.HealthMonitor
{
    public static class DependencyInjection
    {
        public static void AddHealthMonitoring(this IServiceCollection serviceCollection) => serviceCollection.AddSingleton<RequestCounterService>();

        public static void StartHealthMonitoring(this IServiceProvider serviceProvider) => serviceProvider.GetService<RequestCounterService>();
    }
}
