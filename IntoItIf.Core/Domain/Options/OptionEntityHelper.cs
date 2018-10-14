namespace IntoItIf.Core.Domain.Options
{
   using Entities;

   public static class OptionEntityHelper
   {
      #region Public Methods and Operators

      public static Option<TDto> ToDto<T, TDto>(this Option<T> entity)
         where T : class, IEntity
         where TDto : class, IDto
      {
         return entity.MapFlatten(x => x.ToDto<TDto>());
      }

      public static Option<T> ToEntity<TDto, T>(this Option<TDto> dto)
         where TDto : class, IDto
         where T : class, IEntity
      {
         return dto.MapFlatten(x => x.ToEntity<T>());
      }

      #endregion
   }
}