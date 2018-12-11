namespace IntoItIf.Base.Domain.Entities
{
   using System;

   public interface IEntity : IValidationEntity
   {
      #region Public Methods and Operators

      TDto ToDto<TDto>()
         where TDto : class, IDto;

      #endregion
   }

   public interface IEntity<out TKey> : IEntity
      where TKey : IEquatable<TKey>, IComparable<TKey>
   {
      TKey Id { get; }
   }
}