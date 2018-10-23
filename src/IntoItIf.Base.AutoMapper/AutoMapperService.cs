namespace IntoItIf.Base.AutoMapper
{
   using Domain.Entities;
   using Domain.Options;
   using global::AutoMapper;
   using global::AutoMapper.Configuration;
   using Mappers;

   public sealed class AutoMapperService : IMapperService
   {
      public Option<bool> Initialize<T>(params Option<T>[] mapperProfiles)
         where T : class, IMapperProfile
      {
         return mapperProfiles.ToOptionOfArray()
            .Map(
               x =>
               {
                  var config = new MapperConfigurationExpression();
                  foreach (var y in x)
                  {
                     var binds = y.GetBinds();
                     foreach (var bind in binds)
                     {
                        bind.Execute(z => config.CreateMap(z.Source, z.Destination));
                     }
                  }

                  Mapper.Initialize(config);
                  return true;
               });
      }

      public Option<TDto> ToDto<T, TDto>(Option<T> entity)
         where T : class, IEntity where TDto : class, IDto
      {
         return Mapper.Map<T, TDto>(entity.ReduceOrDefault());
      }

      public Option<T> ToEntity<TDto, T>(Option<TDto> dto)
         where TDto : class, IDto where T : class, IEntity
      {
         return Mapper.Map<TDto, T>(dto.ReduceOrDefault());
      }
   }
}