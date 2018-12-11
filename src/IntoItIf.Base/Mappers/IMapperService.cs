namespace IntoItIf.Base.Mappers
{
   using Domain.Entities;
   using Services;

   public interface IMapperService : IInjectable
   {
      #region Public Methods and Operators

      bool Initialize(params IMapperProfile[] mapperProfiles);

      TDto ToDto<T, TDto>(T entity)
         where T : class, IEntity
         where TDto : class, IDto;

      T ToEntity<TDto, T>(TDto dto)
         where TDto : class, IDto
         where T : class, IEntity;

      #endregion
   }
}