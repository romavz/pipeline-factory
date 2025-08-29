using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;
using System;

namespace PipelineFactoryNet.Tests
{
    [TestFixture]
    public class PipelineFactoryBaseTests
    {
        [Test]
        public void GetNewBuilder_ShouldReturnNewBuilder()
        {
            var builder1 = Substitute.For<IPipelineBuilder<object>>();
            var builder2 = Substitute.For<IPipelineBuilder<object>>();
            var builder3 = Substitute.For<IPipelineBuilder<object>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            var factory = new PipelineFactoryInheritor<object>(serviceProvider);
            
            serviceProvider.GetService(Arg.Is(typeof(IPipelineBuilder<object>))).Returns(builder1, builder2, builder3);

            using (new AssertionScope())
            {
                factory.GetNewBuilder().Should().Be(builder1);
                factory.GetNewBuilder().Should().Be(builder2);
                factory.GetNewBuilder().Should().Be(builder3);
            }
        }

        [Test]
        public void GetNewBuilder_WhenPipelineBuilderNotRegistredInDI_ShouldThrowException()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var factory = new PipelineFactoryInheritor<object>(serviceProvider);
            serviceProvider.GetService(Arg.Is(typeof(IPipelineBuilder<object>))).Returns(null);

            Action getNewBuilder = () => factory.GetNewBuilder();
            
            getNewBuilder.Should().Throw<ArgumentException>()
                .WithMessage("Сервис не зарегистрирован (Parameter 'IPipelineBuilder')");
        }
    }
    
    internal class PipelineFactoryInheritor<TContext>: PipelineFactoryBase<TContext>
    {
        public PipelineFactoryInheritor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public new IPipelineBuilder<TContext> GetNewBuilder()
        {
            return base.GetNewBuilder();
        }

        public override IPipeline<TContext> Create()
        {
            throw new NotImplementedException();
        }
    }
}