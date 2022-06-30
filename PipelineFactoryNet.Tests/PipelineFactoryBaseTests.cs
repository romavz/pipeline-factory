using NSubstitute;
using NUnit.Framework;
using System;

namespace PipelineFactoryNet.Tests
{
    [TestFixture]
    public class PipelineFactoryBaseTests
    {
        // GetNewBuilder должен всегда запрашивать новый экземпляр строителя у поставщика DI.
        // GetNewBuilder должен бросить исключение в случае отсутствия запрашиваемого билдера.
        // Use<> должен обратиться к методу GetNewBuilder и вызывать Use<> полученного билдера.
        // Use() должен обратиться к методу GetNewBuilder и вызвать Use() полученного билдера.
        // When(condition, then, @else) должен обратиться к методу GetNewBuilder и вызывать When(condition, then, @else) полученного билдера.
        // When(condition, then) должен обратиться к методу GetNewBuilder и вызвать When(condition, then) полученного билдера.
        [Test]
        public void GetNewBuilder()
        {
            // var builder1 = Substitute.For<IPipelineBuilder<object>>();
            // var builder2 = Substitute.For<IPipelineBuilder<object>>();
            // var builder3 = Substitute.For<IPipelineBuilder<object>>();
            //
            // var serviceProvider = Substitute.For<IServiceProvider>();
            // serviceProvider.GetService(Arg.Is(typeof(IPipelineBuilder<object>))).Returns(builder1, builder2, builder3);
            //
            // var factory = Substitute.ForPartsOf<PipelineFactoryBase<object>>();
            
            
        }
    }
}