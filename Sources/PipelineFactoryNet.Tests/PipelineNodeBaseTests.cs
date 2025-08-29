using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace PipelineFactoryNet.Tests
{
    [TestFixture]
    public class PipelineNodeBaseTests
    {
        [Test]
        public void NextNode_ShouldBe_SentinelNode_ByDefault()
        {
            var sentinelNode = SentinelNode<object>.Instance();
            var pipelineNode = Substitute.ForPartsOf<PipelineNodeBase<object>>();
            
            pipelineNode.Next.Should().BeSameAs(sentinelNode);
        }
        
        [Test]
        public void SetNext_ShouldSetNext()
        {
            var nextNode = Substitute.For<IPipelineNode<object>>();
            var pipelineNode = Substitute.ForPartsOf<PipelineNodeBase<object>>();

            pipelineNode.SetNext(nextNode);

            pipelineNode.Next.Should().Be(nextNode);
        }
    }
}