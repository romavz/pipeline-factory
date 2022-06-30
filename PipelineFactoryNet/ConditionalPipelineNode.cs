using System;
using System.Threading.Tasks;

namespace PipelineFactoryNet
{
    /// <summary>
    /// Конвеерный узел с условием. Содержит условие и ссылки на конвееры, исполняемые при положительном или отрицательном
    /// исходе проверки условия.
    /// </summary>
    /// <typeparam name="TContext">Тип обрабатываемого контекста.</typeparam>
    public class ConditionalPipelineNode<TContext> : PipelineNodeBase<TContext>
    {
        private readonly Func<TContext, bool> _condition;
        private readonly IPipeline<TContext> _positiveLine;
        private readonly IPipeline<TContext> _negativeLine;
        
        /// <summary>
        /// Создает элемент с двумя цепочками обработки контекста, вызываемых в зависимости от исхода проверки условия.
        /// </summary>
        /// <param name="condition">Условная функция, принимающая на вход контекст, возвращающая: true/false.</param>
        /// <param name="positiveLine">Конвеер обработки, вызываемый при положительном исходе проверки условия.</param>
        /// <param name="negativeLine">Конвеер обработки, вызываемый при отрицательном исходе проверки условия.</param>
        public ConditionalPipelineNode(Func<TContext, bool> condition, IPipeline<TContext> positiveLine, IPipeline<TContext> negativeLine)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
            _positiveLine = positiveLine ?? throw new ArgumentNullException(nameof(positiveLine));
            _negativeLine = negativeLine ?? throw new ArgumentNullException(nameof(negativeLine));;
        }

        /// <summary>
        /// Создает элемент с цепочкой обработки контекста, вызываемой при положительном исходе проверки условия.
        /// </summary>
        /// <param name="condition">Условная функция, принимающая на вход контекст, возвращающая: true/false.</param>
        /// <param name="positiveLine">Конвеер обработчиков, вызываемый при положительном исходе проверки условия.</param>
        public ConditionalPipelineNode(Func<TContext, bool> condition, IPipeline<TContext> positiveLine) : 
            this(condition, positiveLine, new SentinelNode<TContext>())
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