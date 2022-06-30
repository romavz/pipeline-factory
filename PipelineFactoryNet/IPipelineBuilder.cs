using System;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Строитель конвеера обработки контекста.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public partial interface IPipelineBuilder<TContext>
    {
        /// <summary>
        /// Добавляет узел в конвеер.
        /// </summary>
        /// <param name="handlers">Ссылки на методы-обработчики контекста.</param>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> Use(params ContextHandler<TContext>[] handlers);

        /// <summary>
        /// Добавляет узел в конвеер.
        /// </summary>
        /// <typeparam name="TContextHandler">Ссылка на конкретный обработчик контекста.</typeparam>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> Use<TContextHandler>() where TContextHandler : IContextHandler<TContext>;

        /// <summary>
        /// Добавляет узел ветвления конвеера по условию.
        /// </summary>
        /// <param name="condition">Условие.</param>
        /// <param name="then">Строитель, используемый, при положительном результате проверки условия.</param>
        /// <param name="else">Строитель, используемуй, при отрицательном результате проверки условия.</param>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> When(Func<TContext, bool> condition, IPipelineBuilder<TContext> @then, IPipelineBuilder<TContext> @else);
        
        /// <summary>
        /// Добавляет узел ветвления конвеера по условию.
        /// </summary>
        /// <param name="condition">Условие.</param>
        /// <param name="then">Строитель, используемый, при положительном результате проверки условия.</param>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> When(Func<TContext, bool> condition, IPipelineBuilder<TContext> @then);

        /// <summary>
        /// Возвращает созданную цепочку.
        /// </summary>
        /// <returns>Ссылка на конвеер обработки контекста.</returns>
        IPipeline<TContext> Create();
    }
}