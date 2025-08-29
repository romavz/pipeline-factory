using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PipelineFactoryNet.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddPipelineFor<TContext>(this IServiceCollection serviceCollection) 
            where TContext : class
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            
            serviceCollection.Scan(select =>
            {
                select.FromAssemblies(callingAssembly)
                    .AddClasses(implementation => implementation.AssignableTo<IPipelineFactory<TContext>>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(implementation => implementation.AssignableTo<IContextHandler<TContext>>())
                    .AsSelf()
                    .WithScopedLifetime()
                    .AddClasses(implementation => implementation.AssignableTo<IPipelineNode<TContext>>())
                    .AsSelf()
                    .WithScopedLifetime();
            });
            
            serviceCollection.AddTransient<IPipelineBuilder<TContext>, PipelineBuilder<TContext>>();
            serviceCollection.AddScoped<IPipeline<TContext>>(x => x.GetService<IPipelineFactory<TContext>>()!.Create());

            return serviceCollection;
        }
    }
}