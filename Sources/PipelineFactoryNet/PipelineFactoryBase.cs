using Microsoft.Extensions.DependencyInjection;
using System;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Базовая реализация фабрики конвейера, реализующая синтаксический сахар для наследников.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class PipelineFactoryBase<TContext> : IPipelineFactory<TContext>
    {
        protected readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PipelineFactoryBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        
        /// <inheritdoc/>
        public abstract IPipeline<TContext> Create();
        
        /// <summary>
        /// Получить новый экземпляр строителя конвейера обработки контекста.
        /// </summary>
        /// <returns>Строитель конвейера обработки контекста.</returns>
        /// <exception cref="ArgumentException"></exception>
        protected IPipelineBuilder<TContext> GetNewBuilder()
        {
            // ServiceProvider должен всегда возвращать новый экземпляр, иначе условные разветвления будут добавлять
            // все узлы из веток в продолжение конвейера вместо веток.
            return ServiceProvider.GetService<IPipelineBuilder<TContext>>() 
                   ?? throw new ArgumentException("Сервис не зарегистрирован", nameof(IPipelineBuilder<TContext>));
        }

        protected IPipelineBuilder<TContext> Use(params ContextHandler<TContext>[] handlers)
        {
            return GetNewBuilder().Use(handlers);
        }

        protected IPipelineBuilder<TContext> Use<TContextHandler>() where TContextHandler : IContextHandler<TContext>
        {
            return GetNewBuilder().Use<TContextHandler>();
        }

        protected IPipelineBuilder<TContext> When(Predicate<TContext> predicate,
            IPipelineBuilder<TContext> then,
            IPipelineBuilder<TContext> @else)
        {
            return GetNewBuilder().When(predicate, then, @else);
        }

        protected IPipelineBuilder<TContext> When(Predicate<TContext> predicate, IPipelineBuilder<TContext> then)
        {
            return GetNewBuilder().When(predicate, then);
        }
    }
}