using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Делегат, представляющий метод обработки контекста, помещаемый в узел конвейера обработчиков. 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public delegate Task ContextHandler<in TContext>(TContext context);
}