using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Обобщенный интерфейс конечного обработчика контекста, вызываемый из конвейера. 
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    /// <remarks>Служит для отделения тестов конвейерных узлов от тестов "полезной нагрузки".</remarks>
    public interface IContextHandler<in TContext>
    {
        /// <summary>
        /// Асинхронный обработчик контекста.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <returns><see cref="Task"/> - для возможности асинхронного выполнения.</returns>
        Task Handle(TContext context);
    }
}