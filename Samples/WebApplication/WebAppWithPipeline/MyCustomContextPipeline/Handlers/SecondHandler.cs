using PipelineFactoryNet;
using System;
using System.Threading.Tasks;
using WebAppWithPipeline.MyCustomContextPipeline;

namespace WebAppWithPipeline
{
    public class SecondHandler : IContextHandler<CustomContext>
    {
        public Task Handle(CustomContext context)
        {
            context.Field2 = $"run {nameof(SecondHandler)}";
            Console.WriteLine(context.Field2);
            context.BoolField = true;
            return Task.CompletedTask;
        }
    }
}