# PipeLineFactoryNet

Простая фабрика создания конвейеров обработки контекста.

## Цель проекта:
    Упростить реализацию последовательности логически выделенных манипуляций над некоторым контекстом и улучшить 
    пригодность к тестированию полученного кода.

## Описание:
    В данной реализации логика конвейера и логика конечных обработчиков отделены друг от друга для вынесения 
    однообразных тестов касающихся передачи управления следующему узлу конвейера в тесты конвейера. 
    Клиенту необходимо писать только простые тесты касающиеся логики его обработчиков.

## Быстрый старт:
    1. Добавить в проект пакет PipelineFactoryNet;
    2. Создать класс обрабатываемого контекста, например, космический корабль - SpaceShip;
    3. Добавить в DI-контейнер зависимости. Всего одна строка в метод ConfigureServices: 
        services.AddPipelineFor<YourContextType>(). Например, контекст определен в класссе SpaceShip:
```c#
    services.AddPipelineFor<SpaceShip>();
```
    4. Создать свои обработчики контекста, с реализацией интерфейса IContextHandler<Spaceship>:
```c#
    public class ModelValidator : IContextHandler<SpaceShip>
    {
        public Task Handle(SpaceShip spaceShip)
        {
            if(spaceShip.Model != SpaceShipModels.BlackStarJumper)
                throw new SpaceShipAssemblingException($"Invalid model - {spaceShip.Model}"); 
        }
    }
```
    5. Далее можно создать юнит-тесты для своих обработчиков;
    6. Реализовать фабрику для создания конвейера, для синтаксического упрощения можно наследоваться от
        PipelineFactoryBase<T> или напрямую реализовать интерфейс IPipelineFactory<T>.
        Допустим, мы реализуем конвейер сборки космического корабля, тогда он мог бы выглядиеть примерно так:
```c#
    public class SpaceShipPipelineFactory : PipelineFactoryBase<SpaceShip>
    {
        public SpaceShipPipelineFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override IPipeline<SpaceShip> Create()
        {
            return GetNewBuilder().
                AddNode<AssemblingExceptionsLogger>()
                .Use<ModelValidator>()
                .Use<FrameBuilder>()
                .When(ship => ship.HasLiquidFuelType(),
                    then: 
                        Use<LiquidFuelSystemInstaller>(), 
                    @else:
                        Use<SolidFuelSystemInstaller>.
                        When(ship => ship.FuelSystem.SomeProperty == "some value", 
                            then:
                                Use(async (_) =>
                                {
                                    Console.WriteLine("Some property = some value");
                                    await Task.CompletedTask;
                                })
                            )
                    )
                .Use<JetsInstaller>()
                .Use<SpaceJumperInstaller>()
                .Use<SpaceShipTester>()
                .Use<SpaceShipCrewAssembler>()
                .Create();
        }
    }
```

    7. Далее, с помощью DI мы можем получить наш конвейер в виде зависимости в нужном нам месте и запустить его с помощью
        метода IPipeline<SpaceShip>.Perform(spaceship), передав ему наш контекст для обработки.
```c#
    {
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IPipeline<SpaceShip> _pipeline;

        public TestController(IPipeline<SpaceShip> pipeline)
        {
            _pipeline = pipeline;
        }

        [HttpGet]
        public async Task<SpaceShip> Index()
        {
            var spaceShip = new SpaceShip();
            spaceShip.Model = SpaceShipModels.Interceptor
            
            await _pipeline.Perform(spaceShip);
            return spaceShip;
        }
    }
}
```
