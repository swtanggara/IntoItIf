namespace IntoItIf.Base.Validations.Must.Internals
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text.RegularExpressions;

   internal static class Is
   {
      public static bool InRange<T>(T comparable, T from, T to) where T : IComparable<T>
      {
         return comparable.CompareTo(from) >= 0 && comparable.CompareTo(to) <= 0;
      }

      public static bool Same(object actual, object expected)
      {
         if (actual == null && expected == null)
            return true;
         if (actual == null || expected == null)
            return false;
         return actual == expected;
      }

      public static bool Equal<T>(T expected, T actual)
      {
         return Equal(expected, actual, GetEqualityComparer<T>(null));
      }

      public static bool Equal<T>(T expected, T actual, IEqualityComparer<T> comparer)
      {
         return comparer.Equals(actual, expected);
      }

      private static IEqualityComparer<T> GetEqualityComparer<T>(
        IEqualityComparer innerComparer = null)
      {
         var typeoft = typeof(T);
         if (typeoft.IsValueType)
         {
            return new EqualityComparer<T>(innerComparer);
         }
         return new ObjectEqualityComparer<T>();
      }

      public static bool Equal<T>(IEnumerable<T> actual, IEnumerable<T> expected)
      {
         if (actual == null && expected == null)
            return true;
         if (actual == null || expected == null)
            return false;
         IEnumerator<T> enumerator1 = expected.GetEnumerator();
         IEnumerator<T> enumerator2 = actual.GetEnumerator();
         bool flag1;
         bool flag2;
         do
         {
            flag1 = enumerator1.MoveNext();
            flag2 = enumerator2.MoveNext();
            if (!flag1 && !flag2)
               return true;
         }
         while (flag1 == flag2 && Equal(enumerator2.Current, enumerator1.Current));
         return false;
      }

      public static bool EqualIgnoreOrder<T>(IEnumerable<T> actual, IEnumerable<T> expected)
      {
         if (actual == null && expected == null)
            return true;
         if (actual == null || expected == null)
            return false;
         ICollection collection = actual as ICollection;
         if (collection != null && expected is ICollection && collection.Count != ((ICollection)expected).Count)
            return false;
         List<T> list = expected.ToList();
         foreach (T obj1 in actual)
         {
            T actualElement = obj1;
            T obj2 = list.FirstOrDefault(x => Equal(x, actualElement));
            if (!list.Remove(obj2))
               return false;
         }
         return !list.Any();
      }

      public static bool Equal(
        IEnumerable<decimal> actual,
        IEnumerable<decimal> expected,
        decimal tolerance)
      {
         IEnumerator<decimal> enumerator1 = expected.GetEnumerator();
         IEnumerator<decimal> enumerator2 = actual.GetEnumerator();
         bool flag1;
         bool flag2;
         do
         {
            flag1 = enumerator1.MoveNext();
            flag2 = enumerator2.MoveNext();
            if (!flag1 && !flag2)
               return true;
         }
         while (flag1 == flag2 && Equal(enumerator2.Current, enumerator1.Current, tolerance));
         return false;
      }

      public static bool Equal(
        IEnumerable<float> actual,
        IEnumerable<float> expected,
        double tolerance)
      {
         IEnumerator<float> enumerator1 = expected.GetEnumerator();
         IEnumerator<float> enumerator2 = actual.GetEnumerator();
         bool flag1;
         bool flag2;
         do
         {
            flag1 = enumerator1.MoveNext();
            flag2 = enumerator2.MoveNext();
            if (!flag1 && !flag2)
               return true;
         }
         while (flag1 == flag2 && Equal(enumerator2.Current, enumerator1.Current, tolerance));
         return false;
      }

      public static bool Equal(
        IEnumerable<double> actual,
        IEnumerable<double> expected,
        double tolerance)
      {
         IEnumerator<double> enumerator1 = expected.GetEnumerator();
         IEnumerator<double> enumerator2 = actual.GetEnumerator();
         bool flag1;
         bool flag2;
         do
         {
            flag1 = enumerator1.MoveNext();
            flag2 = enumerator2.MoveNext();
            if (!flag1 && !flag2)
               return true;
         }
         while (flag1 == flag2 && Equal(enumerator2.Current, enumerator1.Current, tolerance));
         return false;
      }

      public static bool Equal(decimal actual, decimal expected, decimal tolerance)
      {
         return Math.Abs(actual - expected) < tolerance;
      }

      public static bool Equal(double actual, double expected, double tolerance)
      {
         return Math.Abs(actual - expected) < tolerance;
      }

      public static bool Equal(DateTime actual, DateTime expected, TimeSpan tolerance)
      {
         return (actual - expected).Duration() < tolerance;
      }

      public static bool Equal(DateTimeOffset actual, DateTimeOffset expected, TimeSpan tolerance)
      {
         return (actual - expected).Duration() < tolerance;
      }

      public static bool Equal(TimeSpan actual, TimeSpan expected, TimeSpan tolerance)
      {
         return (actual - expected).Duration() < tolerance;
      }

      public static bool InstanceOf(object o, Type expected)
      {
         return o != null && expected.IsInstanceOfType(o);
      }

      public static bool StringMatchingRegex(string actual, string regexPattern)
      {
         return Regex.IsMatch(actual, regexPattern);
      }

      public static bool StringContainingIgnoreCase(string actual, string expected)
      {
         if (actual == null)
            return false;
         return actual.IndexOf(expected, StringComparison.OrdinalIgnoreCase) != -1;
      }

      public static bool StringContainingUsingCaseSensitivity(string actual, string expected)
      {
         if (actual == null)
            return false;
         return actual.IndexOf(expected, StringComparison.Ordinal) != -1;
      }

      public static bool EndsWithUsingCaseSensitivity(
        string actual,
        string expected,
        Case caseSensitivity)
      {
         if (actual == null)
            return false;
         if (caseSensitivity == Case.Insensitive)
            return actual.EndsWith(expected, StringComparison.OrdinalIgnoreCase);
         return actual.EndsWith(expected);
      }

      public static bool StringStartingWithUsingCaseSensitivity(
        string actual,
        string expected,
        Case caseSensitivity)
      {
         if (actual == null)
            return false;
         if (caseSensitivity == Case.Insensitive)
            return actual.StartsWith(expected, StringComparison.OrdinalIgnoreCase);
         return actual.StartsWith(expected);
      }

      public static bool StringEqualWithCaseSensitivity(
        string actual,
        string expected,
        Case caseSensitivity)
      {
         if (caseSensitivity == Case.Insensitive)
            return StringComparer.OrdinalIgnoreCase.Equals(actual, expected);
         return StringComparer.Ordinal.Equals(actual, expected);
      }

      public static bool EnumerableStringEqualWithCaseSensitivity(
        IEnumerable<string> actual,
        IEnumerable<string> expected,
        Case caseSensitivity)
      {
         IEnumerator<string> enumerator1 = expected.GetEnumerator();
         IEnumerator<string> enumerator2 = actual.GetEnumerator();
         bool flag1;
         bool flag2;
         do
         {
            flag1 = enumerator1.MoveNext();
            flag2 = enumerator2.MoveNext();
            if (!flag1 && !flag2)
               return true;
         }
         while (flag1 && flag2 && StringEqualWithCaseSensitivity(enumerator2.Current, enumerator1.Current, caseSensitivity));
         return false;
      }

      public static bool GreaterThanOrEqualTo<T>(T comparable, T expected)
         where T : IComparable<T>
      {
         return Compare(comparable, expected) >= decimal.Zero;
      }

      public static bool GreaterThanOrEqualTo<T>(T actual, T expected, IComparer<T> comparer)
      {
         return Compare(actual, expected, comparer) >= decimal.Zero;
      }

      public static bool LessThanOrEqualTo<T>(T comparable, T expected)
         where T : IComparable<T>
      {
         return Compare(comparable, expected) <= decimal.Zero;
      }

      public static bool LessThanOrEqualTo<T>(T actual, T expected, IComparer<T> comparer)
      {
         return Compare(actual, expected, comparer) <= decimal.Zero;
      }

      public static bool GreaterThan<T>(T comparable, T expected)
         where T : IComparable<T>
      {
         return Compare(comparable, expected) > decimal.Zero;
      }

      public static bool GreaterThan<T>(T actual, T expected, IComparer<T> comparer)
      {
         return Compare(actual, expected, comparer) > decimal.Zero;
      }

      public static bool LessThan<T>(T comparable, T expected)
         where T : IComparable<T>
      {
         return Compare(comparable, expected) < decimal.Zero;
      }

      public static bool LessThan<T>(T actual, T expected, IComparer<T> comparer)
      {
         return Compare(actual, expected, comparer) < decimal.Zero;
      }

      private static decimal Compare<T>(T actual, T expected, IComparer<T> comparer)
      {
         return comparer.Compare(actual, expected);
      }

      private static decimal Compare<T>(T comparable, T expected)
         where T : IComparable<T>
      {
         if (!typeof(T).IsValueType())
         {
            if (comparable == null)
               return (object)expected == null ? 0 : -1;
            if (expected == null)
               return decimal.One;
         }
         return comparable.CompareTo(expected);
      }
   }
}