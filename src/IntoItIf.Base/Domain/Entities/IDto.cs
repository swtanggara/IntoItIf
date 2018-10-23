namespace IntoItIf.Base.Domain.Entities
{
   using Options;

   public interface IDto : IValidationEntity
   {
      #region Public Methods and Operators

      Option<TEntity> ToEntity<TEntity>()
         where TEntity : class, IEntity;

      #endregion
   }
}