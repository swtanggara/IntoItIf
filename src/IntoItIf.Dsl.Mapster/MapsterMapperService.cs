namespace IntoItIf.Dsl.Mapster
{
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using FastExpressionCompiler;
   using global::Mapster;

   public class MapsterMapperService : IMapperService
   {
      #region Public Methods and Operators

      public Option<bool> Initialize<T>(params Option<T>[] mapperProfiles)
         where T : class, IMapperProfile
      {
         return mapperProfiles.ToOptionOfArray()
            .Map(
               x =>
               {
                  foreach (var y in x)
                  {
                     var binds = y.GetBinds();
                     foreach (var bind in binds)
                     {
                        bind.Execute(
                           z => TypeAdapterConfig.GlobalSettings.NewConfig(z.Source, z.Destination));
                     }
                  }

                  TypeAdapterConfig.GlobalSettings.Compiler = y => y.CompileFast();
                  return true;
               });
      }

      public Option<TDto> ToDto<T, TDto>(Option<T> entity)
         where T : class, IEntity where TDto : class, IDto
      {
         return entity.ReduceOrDefault().Adapt<T, TDto>();
      }

      public Option<T> ToEntity<TDto, T>(Option<TDto> dto)
         where TDto : class, IDto where T : class, IEntity
      {
         return dto.ReduceOrDefault().Adapt<TDto, T>();
      }

      #endregion
   }
}