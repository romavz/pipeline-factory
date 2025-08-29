namespace PipelineFactoryNet
{
    /// <summary>
    /// Узел конвейера обработки контекста.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public interface IPipelineNode<TContext> : IPipeline<TContext>
    {
        /// <summary>
        /// Установить следующий узел обработки.
        /// </summary>
        /// <param name="next">Следующий узел обработки.</param>
        void SetNext(IPipelineNode<TContext> next);
    }
}