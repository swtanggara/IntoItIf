namespace IntoItIf.Base.Domain
{
   using System.Collections.Generic;
   using System.Linq;

   public abstract class ValueObject<T>
      where T : ValueObject<T>
   {
      #region Public Methods and Operators

      public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
      {
         if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

         if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

         return a.Equals(b);
      }

      public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
      {
         return !(a == b);
      }

      public override bool Equals(object obj)
      {
         var valueObject = obj as T;

         if (ReferenceEquals(valueObject, null))
            return false;

         return EqualsCore(valueObject);
      }

      public override int GetHashCode()
      {
         return GetEqualityComponents()
            .Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
      }

      public override string ToString()
      {
         return $"({string.Join(", ", GetEqualityComponents())})";
      }

      #endregion

      #region Methods

      protected abstract IEnumerable<object> GetEqualityComponents();

      private bool EqualsCore(ValueObject<T> other)
      {
         return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
      }

      #endregion
   }
}