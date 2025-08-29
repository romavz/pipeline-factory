using System;

namespace PipelineFactoryNet
{
    public interface IPipelineNodeFactory<TContext>
    {
        IPipelineNode<TContext> GetConditionalPipelineNode(Predicate<TContext> predicate, IPipeline<TContext> positiveWay);
        
        IPipelineNode<TContext> GetConditionalPipelineNode(Predicate<TContext> predicate, IPipeline<TContext> positiveWay,
            IPipeline<TContext> negativeWay);

        IPipelineNode<TContext> GetPipelineNode(params ContextHandler<TContext>[] contextHandlers);
    }
}