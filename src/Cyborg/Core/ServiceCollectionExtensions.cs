using Microsoft.Extensions.DependencyInjection;

namespace Cyborg.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(this IServiceCollection services)
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2
        {
            return services
                .AddSingleton<TImplementation>()
                .AddSingleton<TService1>(svc => svc.GetRequiredService<TImplementation>())
                .AddSingleton<TService2>(svc => svc.GetRequiredService<TImplementation>());
        }

        public static IServiceCollection AddEntity<TEntity>(this IServiceCollection services) where TEntity : class, IEntity, new()
        {
            return services.AddTransient(svc =>
            {
                var entityManager = svc.GetRequiredService<IEntityManager>();
                return entityManager.Create<TEntity>();
            });
        }
    }
}