namespace IntoItIf.Core.Domain.Entities
{
   using Options;

   public interface IEntity : IValidationEntity
   {
      #region Public Methods and Operators

      Option<TDto> ToDto<TDto>()
         where TDto : class, IDto;

      #endregion
   }
}