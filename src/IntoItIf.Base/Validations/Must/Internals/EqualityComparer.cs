namespace IntoItIf.Base.Validations.Must.Internals
{
   using System;
   using System.Collections;
   using System.Collections.Generic;

   internal class EqualityComparer<T> : IEqualityComparer<T>
   {
      private static readonly IEqualityComparer DefaultInnerComparer = new EqualityComparerAdapter<object>(new EqualityComparer<object>(null));
      private static readonly Type NullableType = typeof(Nullable<>);
      private readonly Func<IEqualityComparer> _innerComparerFactory;

      public EqualityComparer(IEqualityComparer innerComparer = null)
      {
         _innerComparerFactory = () => innerComparer ?? DefaultInnerComparer;
      }

      public virtual bool Equals(T x, T y)
      {
         Type type = typeof(T);
         if ((object)x == (object)y)
            return true;
         if (!type.IsValueType() || type.IsGenericType() && type.GetGenericTypeDefinition().IsAssignableFrom(NullableType))
         {
            if (Equals(x, (object)default(T)))
               return Equals(y, (object)default(T));
            if (Equals(y, (object)default(T)))
               return false;
         }
         if (Numerics.IsNumericType(x) && Numerics.IsNumericType(y))
         {
            Tolerance empty = Tolerance.Empty;
            return Numerics.AreEqual(x, y, ref empty);
         }
         IEquatable<T> equatable = (object)x as IEquatable<T>;
         if (equatable != null)
            return equatable.Equals(y);
         IComparable<T> comparable1 = (object)x as IComparable<T>;
         if (comparable1 != null)
            return comparable1.CompareTo(y) == 0;
         IComparable comparable2 = (object)x as IComparable;
         if (comparable2 != null)
         {
            try
            {
               return comparable2.CompareTo(y) == 0;
            }
            catch (ArgumentException ex)
            {
            }
         }
         IEnumerable enumerable1 = (object)x as IEnumerable;
         IEnumerable enumerable2 = (object)y as IEnumerable;
         if (enumerable1 == null || enumerable2 == null)
            return Equals(x, (object)y);
         IEnumerator enumerator1 = enumerable1.GetEnumerator();
         IEnumerator enumerator2 = enumerable2.GetEnumerator();
         IEqualityComparer equalityComparer = _innerComparerFactory();
         bool flag1;
         bool flag2;
         do
         {
            flag1 = enumerator1.MoveNext();
            flag2 = enumerator2.MoveNext();
            if (!flag1 || !flag2)
               goto label_18;
         }
         while (equalityComparer.Equals(enumerator1.Current, enumerator2.Current));
         goto label_20;
         label_18:
         return flag1 == flag2;
         label_20:
         return false;
      }

      public virtual int GetHashCode(T obj)
      {
         throw new NotImplementedException();
      }
   }
}