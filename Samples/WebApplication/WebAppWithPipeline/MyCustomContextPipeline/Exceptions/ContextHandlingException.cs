using System;

namespace WebAppWithPipeline.MyCustomContextPipeline.Exceptions
{
    public class ContextHandlingException<TContext> : Exception
    {
        public TContext Context { get; private set; }
        
        public ContextHandlingException(TContext context) : base()
        {
            Context = context;
        }
    }
}