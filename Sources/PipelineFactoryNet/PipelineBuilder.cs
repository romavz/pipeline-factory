using Microsoft.Extensions.DependencyInjection;
using System;

namespace PipelineFactoryNet
{
    /// <inheritdoc/>
    public partial class PipelineBuilder<TContext> : IPipelineBuilder<TContext> where TContext: class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPipelineNode<TContext> _firstNode;
        private IPipelineNode<TContext> _lastNode;
        private readonly IPipelineNodeFactory<TContext> _pipelineNodeFactory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PipelineBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _pipelineNodeFactory = _serviceProvider.GetService<IPipelineNodeFactory<TContext>>() ?? new PipelineNodeFactory<TContext>();
            _firstNode = _pipelineNodeFactory.GetPipelineNode();
            _lastNode = _firstNode;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> AddNode<TNode>() where TNode : IPipelineNode<TContext>
        {
            var node = _serviceProvider.GetService<TNode>()
                       ?? throw new ArgumentException($"Сервис {typeof(TNode).Name} не зарегистрирован.");
            Add(node);
            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Use(params ContextHandler<TContext>[] handlers)
        {
            var newChain = _pipelineNodeFactory.GetPipelineNode(handlers);
            Add(newChain);

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Use<TContextHandler>() where TContextHandler : IContextHandler<TContext>
        {
            var handler = _serviceProvider.GetService<TContextHandler>();
            return Use(handler!.Handle);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> When(Predicate<TContext> predicate, IPipelineBuilder<TContext> then, IPipelineBuilder<TContext> @else)
        {
            var conditionalChainItem = _pipelineNodeFactory.GetConditionalPipelineNode(predicate, then.Create(), @else.Create());
            Add(conditionalChainItem);
            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> When(Predicate<TContext> predicate, IPipelineBuilder<TContext> then)
        {
            var conditionalChainItem = _pipelineNodeFactory.GetConditionalPipelineNode(predicate, then.Create());
            Add(conditionalChainItem);
            return this;
        }

        /// <inheritdoc/>
        public IPipeline<TContext> Create()
        {
            return _firstNode;
        }
        
        /// <summary>
        /// Добавляет новый узел в конвейер.
        /// </summary>
        /// <param name="newNode"></param>
        private void Add(IPipelineNode<TContext> newNode)
        {
            _lastNode.SetNext(newNode);
            _lastNode = newNode;
        }

    }
}