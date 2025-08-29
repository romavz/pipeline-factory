using System;
using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Конвейерный узел с условием. Содержит условие и ссылки на конвейеры, исполняемые при положительном или отрицательном
    /// исходе проверки условия.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public class ConditionalPipelineNode<TContext> : PipelineNodeBase<TContext>
    {
        private readonly Predicate<TContext> _condition;
        private readonly IPipeline<TContext> _positiveLine;
        private readonly IPipeline<TContext> _negativeLine;
        
        /// <summary>
        /// Создает элемент с двумя цепочками обработки контекста, вызываемых в зависимости от исхода проверки условия.
        /// </summary>
        /// <param name="predicate"><see cref="Predicate{T}"/> Условная функция, принимающая на вход контекст, возвращающая: true/false.</param>
        /// <param name="positiveLine">Конвейер обработки, вызываемый при положительном исходе проверки условия.</param>
        /// <param name="negativeLine">Конвейер обработки, вызываемый при отрицательном исходе проверки условия.</param>
        public ConditionalPipelineNode(Predicate<TContext> predicate, IPipeline<TContext> positiveLine, IPipeline<TContext> negativeLine)
        {
            _condition = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _positiveLine = positiveLine ?? throw new ArgumentNullException(nameof(positiveLine));
            _negativeLine = negativeLine ?? throw new ArgumentNullException(nameof(negativeLine));;
        }

        /// <summary>
        /// Создает элемент с цепочкой обработки контекста, вызываемой при положительном исходе проверки условия.
        /// </summary>
        /// <param name="predicate"> <see cref="Predicate{T}"/> Условная функция, принимающая на вход контекст, возвращающая: true/false.</param>
        /// <param name="positiveLine">Конвейер обработчиков, вызываемый при положительном исходе проверки условия.</param>
        public ConditionalPipelineNode(Predicate<TContext> predicate, IPipeline<TContext> positiveLine) : 
            this(predicate, positiveLine, new SentinelNode<TContext>())
        {
        }

        /// <inheritdoc/>
        public override async Task Perform(TContext context)
        {
            var conditionHasPositiveResult = _condition.Invoke(context);
            
            if (conditionHasPositiveResult)
            {
                await _positiveLine.Perform(context);
            }
            else
            {
                await _negativeLine.Perform(context);
            }
            
            await Next.Perform(context);
        }
    }
}