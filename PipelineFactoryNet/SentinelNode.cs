using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Заглушка-предохранитель, чтоб избавиться от проверок при каждом вызове следующего узла конвеера.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    internal class SentinelNode<TContext> : IPipelineNode<TContext>
    {
        /// <inheritdoc/>
        /// <remarks>Ничего не делает, возвращает завершенную задачу.</remarks>
        public Task Perform(TContext context)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        /// <remarks>Ничего не делает. Предназначен для поддержки интерфейса.</remarks>
        public void SetNext(IPipelineNode<TContext> next)
        {
        }
    }
}