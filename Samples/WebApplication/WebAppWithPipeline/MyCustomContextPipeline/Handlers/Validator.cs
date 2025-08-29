using PipelineFactoryNet;
using System;
using System.Threading.Tasks;
using WebAppWithPipeline.MyCustomContextPipeline;

namespace WebAppWithPipeline
{
    /// <summary>
    /// Некоторый обработчик, производящий валидацию полей полученного контекста
    /// </summary>
    public class Validator : IContextHandler<CustomContext>
    {
        public Task Handle(CustomContext context)
        {
            context.Field1 = $"run {nameof(Validator)}";
            Console.WriteLine(context.Field1);
            return Task.CompletedTask;
        }
    }
}