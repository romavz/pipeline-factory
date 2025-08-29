using PipelineFactoryNet;
using System;
using System.Threading.Tasks;
using WebAppWithPipeline.MyCustomContextPipeline;

namespace WebAppWithPipeline
{
        
    public class ContextMutator : IContextHandler<CustomContext>
    {
        public async Task Handle(CustomContext context)
        {
            Console.WriteLine($"run {nameof(ContextMutator)}");
            await Task.CompletedTask;
        }
    }
}