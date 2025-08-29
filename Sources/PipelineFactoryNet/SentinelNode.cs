using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Заглушка-предохранитель, чтоб избавиться от проверок при каждом вызове следующего узла конвейера.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public class SentinelNode<TContext> : IPipelineNode<TContext>
    {
        private static readonly SentinelNode<TContext> NodeInstance = new();

        /// <summary>
        /// Возвращает ссылку на единственный экземпляр класса.
        /// </summary>
        public static IPipelineNode<TContext> Instance()
        {
            return NodeInstance;
        }

        /// <inheritdoc/>
        /// <remarks>Ничего не делает, возвращает завершенную задачу.</remarks>
        public Task Perform(TContext context)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        /// <remarks>Ничего не делает.</remarks>
        public void SetNext(IPipelineNode<TContext> next)
        {
        }
    }
}