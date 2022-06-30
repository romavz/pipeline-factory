using PipelineFactoryNet.Extentions;
using System;

namespace PipelineFactoryNet
{
    /// <inheritdoc/>
    public partial class PipelineBuilder<TContext> : IPipelineBuilder<TContext> where TContext: class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPipelineNode<TContext> _firstNode;
        private IPipelineNode<TContext> _lastNode;
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PipelineBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _firstNode = new PipelineNode<TContext>();
            _lastNode = _firstNode;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Use(params ContextHandler<TContext>[] handlers)
        {
            var newChain = new PipelineNode<TContext>(handlers);
            Add(newChain);

            return this;
        }

        public IPipelineBuilder<TContext> Use<TContextHandler>() where TContextHandler : IContextHandler<TContext>
        {
            var handler = _serviceProvider.GetService<TContextHandler>();
            return Use(handler!.Handle);
        }

        public IPipelineBuilder<TContext> When(Func<TContext, bool> condition, IPipelineBuilder<TContext> @then, IPipelineBuilder<TContext> @else)
        {
            var conditionalChainItem = new ConditionalPipelineNode<TContext>(condition, @then.Create(), @else.Create());
            Add(conditionalChainItem);
            return this;
        }

        public IPipelineBuilder<TContext> When(Func<TContext, bool> condition, IPipelineBuilder<TContext> then)
        {
            var conditionalChainItem = new ConditionalPipelineNode<TContext>(condition, then.Create());
            Add(conditionalChainItem);
            return this;
        }

        /// <inheritdoc/>
        public IPipeline<TContext> Create()
        {
            return _firstNode;
        }
        
        /// <summary>
        /// Добавляет новый узел в конвеер.
        /// </summary>
        /// <param name="newNode"></param>
        protected virtual void Add(IPipelineNode<TContext> newNode)
        {
            _lastNode.SetNext(newNode);
            _lastNode = newNode;
        }

    }
}