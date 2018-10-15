namespace IntoItIf.Core.Domain.Options
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   public static class OptionEnumerableHelper
   {
      #region Public Methods and Operators

      public static bool AllFail<T>(this IEnumerable<Option<T>> sequence)
      {
         return sequence.All(x => x.IsFail());
      }

      public static bool AllNone<T>(this IEnumerable<Option<T>> sequence)
      {
         return sequence.All(x => x.IsNone());
      }

      public static bool AllSome<T>(this IEnumerable<Option<T>> sequence)
      {
         return sequence.All(x => x.IsSome());
      }

      public static bool AnyFail<T>(this IEnumerable<Option<T>> sequence)
      {
         return sequence.Any(x => x.IsFail());
      }

      public static bool AnyNone<T>(this IEnumerable<Option<T>> sequence)
      {
         return sequence.Any(x => x.IsNone());
      }

      public static bool AnySome<T>(this IEnumerable<Option<T>> sequence)
      {
         return sequence.Any(x => x.IsSome());
      }

      public static Option<T> FirstOrNone<T>(
         this IEnumerable<T> sequence,
         Func<T, bool> predicate)
      {
         return sequence.Where(predicate)
            .Select<T, Option<T>>(x => x)
            .DefaultIfEmpty(None.Value)
            .First();
      }

      public static IEnumerable<TResult> Flatten<T, TResult>(
         this IEnumerable<T> sequence,
         Func<T, Option<TResult>> map)
      {
         return sequence.Select(map)
            .OfType<Some<TResult>>()
            .Select(x => (TResult)x);
      }

      #endregion
   }
}