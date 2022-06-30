namespace PipelineFactoryNet
{
    public interface IPipelineFactory<in TContext>
    {
        IPipeline<TContext> Create();
    }
}