using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Конвейер обработки контекста.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public interface IPipeline<in TContext>
    {
        /// <summary>
        /// Метод обработки контекста.
        /// </summary>
        /// <param name="context">Обрабатываемый контекст.</param>
        /// <returns><see cref="Task"/> - для возможности асинхронного вызова.</returns>
        Task Perform(TContext context);
    }
}