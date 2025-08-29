using System;

namespace PipelineFactoryNet
{
    internal class PipelineNodeFactory<TContext> : IPipelineNodeFactory<TContext>
    {
        public IPipelineNode<TContext> GetConditionalPipelineNode(Predicate<TContext> predicate, IPipeline<TContext> positiveWay)
        {
            return new ConditionalPipelineNode<TContext>(predicate, positiveWay);
        }

        public IPipelineNode<TContext> GetConditionalPipelineNode(Predicate<TContext> predicate, IPipeline<TContext> positiveWay,
            IPipeline<TContext> negativeWay)
        {
            return new ConditionalPipelineNode<TContext>(predicate, positiveWay, negativeWay);
        }

        public IPipelineNode<TContext> GetPipelineNode(params ContextHandler<TContext>[] contextHandlers)
        {
            return new PipelineNode<TContext>(contextHandlers);
        }
    }
}