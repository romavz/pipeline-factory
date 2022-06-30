using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipelineFactoryNet.Tests
{
    [TestFixture]
    public class ConditionalPipelineNodeTests
    {
        [Test]
        [TestCase(true, "trueWay invoke")]
        [TestCase(false, "falseWay invoke")]
        public async Task Perform_ShouldInvokeNextNodeAfterConditionalNodes(bool conditionResult, string conditionalWay)
        {
            var trueWay = Substitute.For<IPipeline<object>>();
            var falseWay = Substitute.For<IPipeline<object>>();
            var nextNode = Substitute.For<IPipelineNode<object>>();

            var pipelineNode = new ConditionalPipelineNode<object>((x) => conditionResult, trueWay, falseWay);
            pipelineNode.SetNext(nextNode);
            
            var calls = new List<string>(2); 
            var context = "some context";
            
            trueWay.When(x => x.Perform(context)).Do(x => calls.Add("trueWay invoke"));
            falseWay.When(x => x.Perform(context)).Do(x => calls.Add("falseWay invoke"));
            nextNode.When(x => x.Perform(context)).Do(x => calls.Add("nextNode invoke"));

            await pipelineNode.Perform(context);

            calls.Should().BeEquivalentTo(new[] { conditionalWay, "nextNode invoke" });
        }
    }
}