namespace IntoItIf.Base.Domain.Entities
{
   public interface IDto : IValidationEntity
   {
      #region Public Methods and Operators

      TEntity ToEntity<TEntity>()
         where TEntity : class, IEntity;

      #endregion
   }
}