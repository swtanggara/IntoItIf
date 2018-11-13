namespace IntoItIf.Base.Domain.Entities
{
   using System;
   using Options;

   public interface IEntity : IValidationEntity
   {
      #region Public Methods and Operators

      Option<TDto> ToDto<TDto>()
         where TDto : class, IDto;

      #endregion
   }

   public interface IEntity<out TKey> : IEntity
      where TKey : IEquatable<TKey>, IComparable<TKey>
   {
      TKey Id { get; }
   }
}