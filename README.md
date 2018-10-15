
IntoItIf
===============
[![GitHub](https://img.shields.io/github/license/swtanggara/IntoItIf.svg)](https://github.com/swtanggara/IntoItIf/blob/master/LICENSE)
[![AppVeyor](https://img.shields.io/appveyor/ci/swtanggara/IntoItIf.svg)](https://ci.appveyor.com/project/swtanggara/intoitif)
[![NuGet](https://img.shields.io/nuget/v/IntoItIf.svg)](https://www.nuget.org/packages/IntoItIf/)
[![Github All Releases](https://img.shields.io/github/downloads/swtanggara/IntoItIf/total.svg)](https://github.com/swtanggara/IntoItIf/releases)
[![Github Releases](https://img.shields.io/github/downloads/swtanggara/IntoItIf/latest/total.svg)](https://github.com/swtanggara/IntoItIf/releases/latest)

It's kinda Unit of Work, Repository things, done intuitively in EF **AND** EF Core.

### Setting it Up
You must setting up your `DbContext` class first, either by inheriting `EfCoreDbContext` (EF Core) or `EfDbContext` (EF):

    public class MyDbContext : EfCoreDbContext // Inherit from EfDbContext if you are using EF6 or above
    {
	   public DbSet<MyEntity> Entities { get; set; }
    }

where `MyEntity` must inherit from `IEntity` or from base templating class `BaseEntity<TEntity>`:

    public class MyEntity : BaseEntity<MyEntity>
    {
	   [Key]
       public int Id { get; set; }
       
       public string Name { get; set; }
    }

next, define your `MyDto` class for mapping from `MyEntity`. This `MyDto` class must inherit from `IDto`, or better from `BaseDto<TDto, TValidator>` class:

    public class MyDto: BaseDto<MyDto, MyDtoFluentValidator>
    {
       public int Id { get; set; }
       public string Name { get; set; }
    }

don't forget to define you `MyDto` validator class, by inheriting `BaseFluentValidator<T>` (using [`FluentValidator`](https://github.com/JeremySkinner/FluentValidation)) or `BaseValitValidator<T>` (using [`Valit`](https://github.com/valit-stack/Valit)):

    public class MyDtoFluentValidator: BaseFluentValidator<MyDto>
    {
       public MyDtoFluentValidator()
       {
          RulesFor(x => x.Id).NotEmpty();
          RulesFor(x => x.Name).NotEmpty();
       }
    }

(`BaseValitValidator<T>` version):

    public class MyDtoValitValidator : BaseValitValidator<MyDto>
    {
       public MyDtoValitValitator()
       {
          Valitator = ValitRules<MyDto>.Create()
            .Ensure(x => x.Id, x => x.IsNonZero())
            .Ensure(x => x.Name, x => x.Required())
            .CreateValitator();
       }
       
       protected override IValitator<MyDto> Valitator { get; }
    }

next, create you `IMapperProfile` derived class to maps `MyEntity` to `MyDto` and vice-versa:

    public class MyMapperProfile : IMapperProfile
    {
       public Option<(Type Source, Type Destination)>[] GetBinds()
       {
          return new Option<(Type Source, Type Destination)>[]
          {
             (typeof(MyEntity), typeof(MyDto)),
             (typeof(MyDto), typeof(MyEntity)),
          };
       }
    }

And, lastly, at your *startup* class, inject the required services like so:

    var mapperSvc = new AutoMapperService(); // Choose between AutoMapperService, BatMapMapperService, MapsterMapperService, or TinyMapperService.
    mapperSvc.Initialize<IMapperProfile>(new MyMapperProfile());
    DslInjecterGetter.SetBaseMapperService(mapperSvc);
    DslInjecterGetter.SetBaseUnitOfWork(new EfCoreUnitOfWork(new MyDbContext())); // Or use EfUnitOfWork, if you are using EF6 or above.

### Usage
It's quite daunting to setting it up huh? But wait, this is how you can utilize my charming library:

    Option<CancellationToken> ctok = CancellationToken.None;
    var dto = new MyDto();
    
    var createResult = Create<MyEntity, MyDto, MyCreateInterceptor>.Handle(dto, ctok);
    var deleteResult = Delete<MyEntity, MyDto, MyDeleteInterceptor>.Handle(dto, ctok);
    var readLookupResult = ReadLookup<MyEntity, MyDto, MyReadLookupInterceptor>.Handle(false, ctok);
    var readOneResult = ReadOne<MyEntity, MyDto, MyReadOneInterceptor>.Handle(dto, ctok);
    var readPagedResult = ReadPaged<MyEntity, MyDto, MyReadPagedInterceptor>.Handle(1, 1, null, "Bla", ctok);
    var updateResult = Update<MyEntity, MyDto, MyUpdateInterceptor>.Handle(dto, ctok);

Yes, of course, you will ask what `MyCreateInterceptor`, `MyDeleteInterceptor`, `MyReadLookupInterceptor`, 
`MyReadOneInterceptor`, `MyReadOneInterceptor`, `MyReadPagedInterceptor`, and `MyUpdateInterceptor` are all about. It's your task to find what they are....
