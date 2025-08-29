using PipelineFactoryNet;
using System.Threading.Tasks;
using WebAppWithPipeline.MyCustomContextPipeline;

namespace WebAppWithPipeline
{
        
    public class ResultHandler : IContextHandler<CustomContext>
    {
        public Task Handle(CustomContext context)
        {
            context.Result = "Some result";
            return Task.CompletedTask;
        }
    }
}