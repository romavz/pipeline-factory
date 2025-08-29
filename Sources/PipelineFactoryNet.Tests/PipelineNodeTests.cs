using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace PipelineFactoryNet.Tests
{
    [TestFixture]
    public class PipelineNodeTests
    {
        [Test]
        public async Task PerformShouldInvokeEachHandler()
        {
            // arrange.
            var context = "some context";
            
            var contextHandlers = new []
            {
                Substitute.For<ContextHandler<string>>(),
                Substitute.For<ContextHandler<string>>(),
                Substitute.For<ContextHandler<string>>(),
            };
            
            var pipelineNode = new PipelineNode<string>(contextHandlers);
            
            // act.
            await pipelineNode.Perform(context);

            // assert.
            Received.InOrder(() =>
            {
                contextHandlers[0].Invoke(context);
                contextHandlers[1].Invoke(context);
                contextHandlers[2].Invoke(context);
            });
        }

        [Test]
        public async Task Perform_ShouldCallNextNodeAfterHandlersInvocation()
        {
            // arrange.
            var context = "some context";
            var nextNode = Substitute.For<IPipelineNode<string>>();

            var contextHandler = Substitute.For<ContextHandler<string>>();

            var pipelineNode = new PipelineNode<string>(contextHandler);
            pipelineNode.SetNext(nextNode);
            
            // act.
            await pipelineNode.Perform(context);

            // assert.
            Received.InOrder(() =>
            {
                contextHandler.Invoke(context);
                nextNode.Perform(context);
            });
        }
        
    }
}