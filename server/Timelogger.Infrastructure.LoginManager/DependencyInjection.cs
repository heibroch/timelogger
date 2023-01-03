using Microsoft.Extensions.DependencyInjection;

namespace Timelogger.Infrastructure.LoginManager
{
    public static class DependencyInjection
    {
        public static void AddLoginService(this IServiceCollection serviceCollection) => serviceCollection.AddSingleton<LoginService>();

        public static void StartLoginService(this IServiceProvider serviceProvider) => serviceProvider.GetService<LoginService>();
    }
}
