using Microsoft.Extensions.Logging;
using PipelineFactoryNet;
using System;
using System.Threading.Tasks;
using WebAppWithPipeline.MyCustomContextPipeline.Exceptions;

namespace WebAppWithPipeline.MyCustomContextPipeline.Nodes
{
    /// <summary>
    /// Специфичный для проекта узел конвейера, в данном случае это логер исключений,
    /// произошедших в ходе обработки контекста.
    /// </summary>
    public class ExceptionsLogger : PipelineNodeBase<CustomContext>
    {
        private readonly ILogger<ExceptionsLogger> _logger;

        public ExceptionsLogger(ILogger<ExceptionsLogger> logger)
        {
            _logger = logger;
        }
        
        public override async Task Perform(CustomContext context)
        {
            try
            {
                await Next.Perform(context);
            }
            catch (ContextHandlingException<CustomContext> e)
            {
                _logger.LogInformation(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}