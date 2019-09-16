namespace IntoItIf.Base.Validations.Must.Internals
{
   using System;
   using System.Collections;
   using System.Collections.Generic;

   internal class EqualityComparerAdapter<T> : IEqualityComparer
   {
      private readonly IEqualityComparer<T> _innerComparer;

      public EqualityComparerAdapter(IEqualityComparer<T> innerComparer)
      {
         _innerComparer = innerComparer;
      }

      public bool Equals(object x, object y)
      {
         return _innerComparer.Equals((T)x, (T)y);
      }

      public int GetHashCode(object obj)
      {
         throw new NotImplementedException();
      }
   }
}