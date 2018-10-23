namespace IntoItIf.Base.Mappers
{
   using Domain.Entities;
   using Domain.Options;
   using Services;

   public interface IMapperService : IInjectable
   {
      #region Public Methods and Operators

      Option<bool> Initialize<T>(params Option<T>[] mapperProfiles)
         where T : class, IMapperProfile;

      Option<TDto> ToDto<T, TDto>(Option<T> entity)
         where T : class, IEntity
         where TDto : class, IDto;

      Option<T> ToEntity<TDto, T>(Option<TDto> dto)
         where TDto : class, IDto
         where T : class, IEntity;

      #endregion
   }
}