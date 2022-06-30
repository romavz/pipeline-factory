using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Обобщенный интерфейс конечного обработчика контекста, вызываемый из конвеера. 
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    /// <remarks>Служит для отделения тестов конвеерных узлов от тестов "полезной нагрузки".</remarks>
    public interface IContextHandler<in TContext>
    {
        /// <summary>
        /// Асинхронный обработчик контекста.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <returns><see cref="Task"/> - для возможности ассинхронного выполнения.</returns>
        Task Handle(TContext context);
    }
}