using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace PipelineFactoryNet.Tests
{
    [TestFixture]
    public class PipelineBuilderTests
    {
        [Test]
        public void AddNode_ShouldAddNodeToEndOfPipeline()
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var customNode = Substitute.For<IPipelineNode<object>>();
            var addedNode = (IPipelineNode<object>)null;

            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);
            serviceProvider.GetService<IPipelineNode<object>>().Returns(customNode);

            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);

            nodeWithoutHandlers.When(x => x.SetNext(customNode))
                .Do(x => addedNode = x.ArgAt<IPipelineNode<object>>(position: 0));
            
            // act.
            pipelineBuilder.AddNode<IPipelineNode<object>>();

            // assert.
            addedNode.Should().Be(customNode);
        }
        
        
        [Test]
        public void Create_BeforeAnyOtherMethods_ShouldReturnPipelineWithEmptyNode()
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);

            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);

            // act.
            var pipeline = pipelineBuilder.Create();

            // assert.
            pipeline.Should().Be(nodeWithoutHandlers);
        }

        [Test]
        public void Use_WithHandler_ShouldAddNewPipelineNode()
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var expectedNode = Substitute.For<IPipelineNode<object>>();
            var addedNode = (IPipelineNode<object>)null;
            
            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            var contextHandler = new ContextHandler<object>((context => Task.CompletedTask));
            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            nodeFactory.GetPipelineNode(contextHandler).Returns(expectedNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);
            
            nodeWithoutHandlers.When(x => x.SetNext(expectedNode))
                .Do(x => addedNode = x.ArgAt<IPipelineNode<object>>(position: 0));
            
            // act.
            pipelineBuilder.Use(contextHandler);
            
            // assert.
            addedNode.Should().Be(expectedNode);
        }

        [Test]
        public void GenericUse_WithHandlerType_ShouldAddNewPipelineNode()
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var expectedNode = Substitute.For<IPipelineNode<object>>();
            var nextNode = (IPipelineNode<object>)null;
           
            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            nodeFactory.GetPipelineNode(Arg.Any<ContextHandler<object>>()).Returns(expectedNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);

            var contextHandler = Substitute.For<IContextHandler<object>>();
            serviceProvider.GetService<IContextHandler<object>>().Returns(contextHandler);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);
            
            nodeWithoutHandlers.When(x => x.SetNext(expectedNode))
                .Do(x => nextNode = x.ArgAt<IPipelineNode<object>>(position: 0));
            
            // act.
            pipelineBuilder.Use<IContextHandler<object>>();
            
            // assert.
            nextNode.Should().Be(expectedNode);
        }

        [Test]
        public void Use_ShouldReturnLinkToThisBuilder()
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var otherNode = Substitute.For<IPipelineNode<object>>();

            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            var contextHandler = new ContextHandler<object>((context => Task.CompletedTask));
            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            nodeFactory.GetPipelineNode(contextHandler).Returns(otherNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);

            // act.
            var nextBuilder = pipelineBuilder.Use(contextHandler);
            
            // assert.
            nextBuilder.Should().Be(pipelineBuilder);
        }
        
        [Test]
        public void GenericUse_ShouldReturnLinkToThisBuilder()
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var otherNode = Substitute.For<IPipelineNode<object>>();

            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            nodeFactory.GetPipelineNode(Arg.Any<ContextHandler<object>>()).Returns(otherNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);

            var contextHandler = Substitute.For<IContextHandler<object>>();
            serviceProvider.GetService<IContextHandler<object>>().Returns(contextHandler);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);
            
            // act.
            var nextBuilder = pipelineBuilder.Use<IContextHandler<object>>();
            
            // assert.
            nextBuilder.Should().Be(pipelineBuilder);
        }

        [Test]
        [TestCase(true, TestName = "When predicate result is True")]
        [TestCase(false, TestName = "When predicate result is False")]
        public void When_WithOnlyPositiveWayBuilder_ShouldAddConditionalNode(bool predicateResult)
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var secondNode = (IPipelineNode<object>)null;

            var conditionalNode = Substitute.For<IPipelineNode<object>>();
            var positiveWay = Substitute.For<IPipelineNode<object>>();
            var negativeWay = Substitute.For<IPipelineNode<object>>();

            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            var positiveWayBuilder = Substitute.For<IPipelineBuilder<object>>();
            var negativeWayBuilder = Substitute.For<IPipelineBuilder<object>>();

            positiveWayBuilder.Create().Returns(positiveWay);
            negativeWayBuilder.Create().Returns(negativeWay);

            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);

            nodeFactory.GetConditionalPipelineNode(Arg.Any<Predicate<object>>(), positiveWay, negativeWay)
                .Returns(conditionalNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);
            
            nodeWithoutHandlers.When(x => x.SetNext(conditionalNode))
                .Do(x => secondNode = x.ArgAt<IPipelineNode<object>>(position: 0));
            
            // act.
            pipelineBuilder.When(x => predicateResult, then: positiveWayBuilder, @else: negativeWayBuilder);
            
            // assert.
            secondNode.Should().Be(conditionalNode);
        }

        [Test]
        [TestCase(true, TestName = "When predicate result is True")]
        [TestCase(false, TestName = "When predicate result is False")]
        public void When_WithPositiveAndNegativeWaysBuilders_ShouldAddConditionalNode(bool predicateResult)
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var secondNode = (IPipelineNode<object>)null;

            var conditionalNode = Substitute.For<IPipelineNode<object>>();
            var conditionalNodePositiveWay = Substitute.For<IPipelineNode<object>>();

            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            var positiveWayBuilder = Substitute.For<IPipelineBuilder<object>>();

            positiveWayBuilder.Create().Returns(conditionalNodePositiveWay); 

            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);

            nodeFactory.GetConditionalPipelineNode(Arg.Any<Predicate<object>>(), conditionalNodePositiveWay)
                .Returns(conditionalNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);
            
            nodeWithoutHandlers.When(x => x.SetNext(conditionalNode))
                .Do(x => secondNode = x.ArgAt<IPipelineNode<object>>(position: 0));
            
            // act.
            pipelineBuilder.When(x => predicateResult, positiveWayBuilder);
            
            // assert.
            secondNode.Should().Be(conditionalNode);
        }

        [Test]
        [TestCase(true, TestName = "When predicate result is True")]
        [TestCase(false, TestName = "When predicate result is False")]
        public void When_WithOnlyPositiveWayBuilder_ShouldReturnLinkToThisBuilder(bool predicateResult)
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var otherNode = Substitute.For<IPipelineNode<object>>();

            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            nodeFactory.GetPipelineNode(Arg.Any<ContextHandler<object>>()).Returns(otherNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);

            var contextHandler = Substitute.For<IContextHandler<object>>();
            serviceProvider.GetService<IContextHandler<object>>().Returns(contextHandler);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);

            var positiveWayBuilder = Substitute.For<IPipelineBuilder<object>>();
            
            // act.
            var nextBuilder = pipelineBuilder.When(x => predicateResult, then: positiveWayBuilder);
            
            // assert.
            nextBuilder.Should().Be(pipelineBuilder);
        }
        
        [Test]
        [TestCase(true, TestName = "When predicate result is True")]
        [TestCase(false, TestName = "When predicate result is False")]
        public void When_WithPositiveAndNegativeWaysBuilders_ShouldReturnLinkToThisBuilder(bool predicateResult)
        {
            // arrange.
            var nodeWithoutHandlers = Substitute.For<IPipelineNode<object>>();
            var otherNode = Substitute.For<IPipelineNode<object>>();

            var nodeFactory = Substitute.For<IPipelineNodeFactory<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            nodeFactory.GetPipelineNode().Returns(nodeWithoutHandlers);
            nodeFactory.GetPipelineNode(Arg.Any<ContextHandler<object>>()).Returns(otherNode);

            serviceProvider.GetService<IPipelineNodeFactory<object>>().Returns(nodeFactory);

            var contextHandler = Substitute.For<IContextHandler<object>>();
            serviceProvider.GetService<IContextHandler<object>>().Returns(contextHandler);
            var pipelineBuilder = new PipelineBuilder<object>(serviceProvider);

            var positiveWayBuilder = Substitute.For<IPipelineBuilder<object>>();
            var negativeWayBuilder = Substitute.For<IPipelineBuilder<object>>();
            
            // act.
            var nextBuilder = pipelineBuilder.When(x => predicateResult, then: positiveWayBuilder, @else: negativeWayBuilder);
            
            // assert.
            nextBuilder.Should().Be(pipelineBuilder);
        }
    }
}