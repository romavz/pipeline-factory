using System;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Строитель конвейера обработки контекста.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public partial interface IPipelineBuilder<TContext>
    {
        /// <summary>
        /// Добавляет новый узел в конвейер. Для возможности добавления специфических узлов с нестандартным
        /// поведением.
        /// </summary>
        IPipelineBuilder<TContext> AddNode<TNode>() where TNode : IPipelineNode<TContext>;

        /// <summary>
        /// Добавляет узел, содержащий несколько обработчиков контекста в конвейер.
        /// </summary>
        /// <param name="handlers">Ссылки на методы-обработчики контекста.</param>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> Use(params ContextHandler<TContext>[] handlers);

        /// <summary>
        /// Добавляет узел, содержащий обработчик контекста в конвейер.
        /// </summary>
        /// <typeparam name="TContextHandler">Ссылка на конкретный обработчик контекста.</typeparam>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> Use<TContextHandler>() where TContextHandler : IContextHandler<TContext>;

        /// <summary>
        /// Добавляет узел ветвления конвейера по условию.
        /// </summary>
        /// <param name="predicate"><seealso cref="Predicate{T}"/> Условный предикат.</param>
        /// <param name="then">Строитель, используемый, при положительном результате проверки условия.</param>
        /// <param name="else">Строитель, используемуй, при отрицательном результате проверки условия.</param>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> When(Predicate<TContext> predicate, IPipelineBuilder<TContext> @then, IPipelineBuilder<TContext> @else);
        
        /// <summary>
        /// Добавляет узел ветвления конвейера по условию.
        /// </summary>
        /// <param name="predicate"><see cref="Predicate{T}"/> Условный предикат.</param>
        /// <param name="then">Строитель, используемый, при положительном результате проверки условия.</param>
        /// <returns>Ссылка на себя.</returns>
        IPipelineBuilder<TContext> When(Predicate<TContext> predicate, IPipelineBuilder<TContext> @then);

        /// <summary>
        /// Возвращает созданную цепочку.
        /// </summary>
        /// <returns>Ссылка на конвейер обработки контекста.</returns>
        IPipeline<TContext> Create();
    }
}