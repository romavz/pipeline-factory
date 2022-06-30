using System;
using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Базовая реализация участка конвеера обработчики контекста.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public abstract class PipelineNodeBase<TContext> : IPipelineNode<TContext>
    {
        /// <summary>
        /// Следующее звено.
        /// </summary>
        public IPipelineNode<TContext> Next { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected PipelineNodeBase()
        {
            Next = new SentinelNode<TContext>();
        }
        
        /// <inheritdoc/>
        public abstract Task Perform(TContext context);
        
        /// <inheritdoc/>
        public virtual void SetNext(IPipelineNode<TContext> next)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
        }
    }
}