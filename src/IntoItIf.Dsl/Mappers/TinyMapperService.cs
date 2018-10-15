namespace IntoItIf.Dsl.Mappers
{
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Nelibur.ObjectMapper;

   public class TinyMapperService : IMapperService
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
                        bind.Execute(z => TinyMapper.Bind(z.Source, z.Destination));
                     }
                  }

                  return true;
               });
      }

      public Option<TDto> ToDto<T, TDto>(Option<T> entity)
         where T : class, IEntity where TDto : class, IDto
      {
         return TinyMapper.Map<T, TDto>(entity.ReduceOrDefault());
      }

      public Option<T> ToEntity<TDto, T>(Option<TDto> dto)
         where TDto : class, IDto where T : class, IEntity
      {
         return TinyMapper.Map<TDto, T>(dto.ReduceOrDefault());
      }

      #endregion
   }
}