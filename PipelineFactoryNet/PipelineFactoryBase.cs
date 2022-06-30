using PipelineFactoryNet.Extentions;
using System;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Базовая реализация фабрики конвеера, реализующая синтаксический сахар для наследников.
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
        /// Получить новый экземпляр строителя конвеера обработки контекста.
        /// </summary>
        /// <returns>Строитель конвеера обработки контекста.</returns>
        /// <exception cref="ArgumentException"></exception>
        protected IPipelineBuilder<TContext> GetNewBuilder()
        {
            // ServiceProvider должен всегда возвращать новый экземпляр, иначе условные разветвления будут добавлять
            // все узлы из веток в продолжение конвеера вместо веток.
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

        protected IPipelineBuilder<TContext> When(Func<TContext, bool> condition,
            IPipelineBuilder<TContext> then,
            IPipelineBuilder<TContext> @else)
        {
            return GetNewBuilder().When(condition, then, @else);
        }

        protected IPipelineBuilder<TContext> When(Func<TContext, bool> condition, IPipelineBuilder<TContext> then)
        {
            return GetNewBuilder().When(condition, then);
        }
    }
}