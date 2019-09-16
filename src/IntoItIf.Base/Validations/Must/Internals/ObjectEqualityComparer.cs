namespace IntoItIf.Base.Validations.Must.Internals
{
   using System;

   [Serializable]
   internal class ObjectEqualityComparer<T> : EqualityComparer<T>
   {
      #region Public Methods and Operators

      public override bool Equals(T x, T y)
      {
         if (x != null)
            return y != null && x.Equals(y);
         return y == null;
      }

      public override bool Equals(object obj)
      {
         return obj is ObjectEqualityComparer<T>;
      }

      public override int GetHashCode(T obj)
      {
         return obj.GetHashCode();
      }

      public override int GetHashCode()
      {
         return GetType().Name.GetHashCode();
      }

      #endregion
   }
}