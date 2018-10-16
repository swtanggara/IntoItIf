namespace IntoItIf.Dsl.BatMap
{
   using System;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using global::BatMap;

   public class BatMapMapperService : IMapperService
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
                        bind.Execute(z => Mapper.RegisterMap(z.Source, z.Destination));
                     }
                  }

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
         try
         {
            var result = Mapper.Map<TDto, T>(dto.ReduceOrDefault());
            return result;
         }
         catch (Exception ex)
         {
            return Fail<T>.Throw(ex);
         }
      }

      #endregion
   }
}