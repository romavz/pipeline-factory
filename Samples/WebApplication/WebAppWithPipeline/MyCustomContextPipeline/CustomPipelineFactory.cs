using PipelineFactoryNet;
using System;
using System.Threading.Tasks;
using WebAppWithPipeline.MyCustomContextPipeline.Nodes;

namespace WebAppWithPipeline.MyCustomContextPipeline
{
    public class CustomFactory : PipelineFactoryBase<CustomContext>
    {
        public CustomFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override IPipeline<CustomContext> Create()
        {
            return GetNewBuilder().
                AddNode<ExceptionsLogger>()
                .Use<Validator>()
                .Use<SecondHandler>()
                .When(context => context.BoolField == true,
                    then: 
                        Use<ContextMutator>(), 
                    @else:
                        Use(context => { Console.WriteLine("Else way"); return Task.CompletedTask; },
                                context => Task.CompletedTask
                                ).
                        When(context => context.Field2 == "some value", 
                            then:
                                Use(async (_) =>
                                {
                                    Console.WriteLine("Field 2 = some value");
                                    await Task.CompletedTask;
                                })
                            )
                    )
                .Use<ResultHandler>()
                .Use(x =>
                {
                    Console.WriteLine(x.Result);
                    return Task.CompletedTask;
                })
                .Create();
        }
    }
    
    
}