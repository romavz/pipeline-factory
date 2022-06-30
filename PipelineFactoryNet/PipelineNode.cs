using System;
using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Простой узел конвеера обработки контекста, содержит в себе ссылки на конкретные методы обработчики контекста.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public class PipelineNode<TContext> : PipelineNodeBase<TContext>
    {
        /// <summary>
        /// Делегаты-обработчики контекста.
        /// </summary>
        private ContextHandler<TContext>[] ContextHandlers { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="contextHandlers"> Делегаты, которые по очереди бутут обрабатывать контекст.</param>
        public PipelineNode(params ContextHandler<TContext>[] contextHandlers)
        {
            ContextHandlers = contextHandlers ?? throw new ArgumentNullException(nameof(contextHandlers));
        }
        
        /// <inheritdoc/>
        public override async Task Perform(TContext context)
        {
            foreach (var handler in ContextHandlers)
            {
                await handler.Invoke(context);
            }
            
            await Next.Perform(context);
        }
    }
}