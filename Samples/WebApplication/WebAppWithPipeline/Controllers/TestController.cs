using Microsoft.AspNetCore.Mvc;
using PipelineFactoryNet;
using System.Threading.Tasks;
using WebAppWithPipeline.MyCustomContextPipeline;

namespace WebAppWithPipeline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IPipeline<CustomContext> _pipeline;

        public TestController(IPipeline<CustomContext> pipeline)
        {
            _pipeline = pipeline;
        }

        [HttpGet]
        public async Task<CustomContext> Index()
        {
            var context = new CustomContext();
            await _pipeline.Perform(context);
            return context;
        }
    }
}