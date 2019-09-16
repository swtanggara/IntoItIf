namespace IntoItIf.Base.Validations.Must
{
   using System;
   using System.Collections.Generic;
   using System.Runtime.CompilerServices;
   using Domain;

   internal class ToRange<T, TProp> : ValueObject<ToRange<T, TProp>>
      where TProp : IComparable<TProp>
   {
      #region Constructors and Destructors

      internal ToRange(PropertyMustBe<T, TProp> actual, TProp @from, TProp to)
      {
         Actual = actual;
         From = @from;
         To = to;
      }

      #endregion

      #region Public Properties

      public PropertyMustBe<T, TProp> Actual { get; }
      public TProp From { get; }
      public TProp To { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Actual;
         yield return From;
         yield return To;
      }

      #endregion
   }

   internal class ToComparable<T, TProp> : ValueObject<ToComparable<T, TProp>>
      where TProp : IComparable<TProp>
   {
      #region Constructors and Destructors

      internal ToComparable(PropertyMustBe<T, TProp> actual, TProp expected)
      {
         Actual = actual;
         Expected = expected;
      }

      #endregion

      #region Properties

      internal PropertyMustBe<T, TProp> Actual { get; }
      internal TProp Expected { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Actual;
         yield return Expected;
      }

      #endregion
   }

   internal class ToCompare<T, TProp> : ValueObject<ToCompare<T, TProp>>
   {
      #region Constructors and Destructors

      internal ToCompare(PropertyMustBe<T, TProp> actual, TProp expected)
      {
         Actual = actual;
         Expected = expected;
      }

      #endregion

      #region Properties

      internal PropertyMustBe<T, TProp> Actual { get; }
      internal TProp Expected { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Actual;
         yield return Expected;
      }

      #endregion
   }

   internal class ToCompareEnumerable<T, TProp> : ValueObject<ToCompareEnumerable<T, TProp>>
   {
      #region Constructors and Destructors

      internal ToCompareEnumerable(PropertyMustBe<T, TProp> actual, IEnumerable<TProp> expected)
      {
         Actual = actual;
         Expected = expected;
      }

      #endregion

      #region Properties

      internal PropertyMustBe<T, TProp> Actual { get; }
      internal IEnumerable<TProp> Expected { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Actual;
         yield return Expected;
      }

      #endregion
   }

   internal static class MustReturn
   {
      #region Methods

      internal static ToRange<T, TProp> RangedWith<T, TProp>(this PropertyMustBe<T, TProp> actual, TProp from, TProp to)
         where TProp : IComparable<TProp>
      {
         return new ToRange<T, TProp>(actual, from, to);
      }

      internal static ToComparable<T, TProp> CompareTo<T, TProp>(this PropertyMustBe<T, TProp> actual, TProp expected)
         where TProp : IComparable<TProp>
      {
         return new ToComparable<T, TProp>(actual, expected);
      }

      internal static PropertyMustBe<T, TProp> Is<T, TProp>(
         this ToRange<T, TProp> toRange,
         Func<TProp, TProp, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
         where TProp : IComparable<TProp>
      {
         if (comparer(toRange.Actual.Value, toRange.From, toRange.To))
         {
            return toRange.Actual;
         }

         throw new MustBeException<T, TProp>(toRange.Actual, (toRange.From, toRange.To), mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> IsNot<T, TProp>(
         this ToRange<T, TProp> toRange,
         Func<TProp, TProp, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
         where TProp : IComparable<TProp>
      {
         if (!comparer(toRange.Actual.Value, toRange.From, toRange.To))
         {
            return toRange.Actual;
         }

         throw new MustBeException<T, TProp>(toRange.Actual, toRange, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> Is<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         Func<TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (comparer(actual.Value))
         {
            return actual;
         }

         throw new MustBeException<T, TProp>(actual, null, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> Is<T, TProp>(
         this ToCompare<T, TProp> toCompare,
         Func<TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (comparer(toCompare.Actual.Value))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, null, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> Is<T, TProp>(
         this ToCompare<T, TProp> toCompare,
         Func<TProp, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (comparer(toCompare.Actual.Value, toCompare.Expected))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, toCompare.Expected, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> Is<T, TProp>(
         this ToCompareEnumerable<T, TProp> toCompare,
         Func<TProp, IEnumerable<TProp>, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (comparer(toCompare.Actual.Value, toCompare.Expected))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, toCompare.Expected, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> Is<T, TProp>(
         this ToCompareEnumerable<T, TProp> toCompare,
         Func<IEnumerable<TProp>, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (comparer(toCompare.Expected, toCompare.Actual.Value))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, toCompare.Expected, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> Is<T, TProp>(
         this ToComparable<T, TProp> toCompare,
         Func<TProp, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
         where TProp : IComparable<TProp>
      {
         if (comparer(toCompare.Actual.Value, toCompare.Expected))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, toCompare.Expected, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> IsNot<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         Func<TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (!comparer(actual.Value))
         {
            return actual;
         }

         throw new MustBeException<T, TProp>(actual, null, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> IsNot<T, TProp>(
         this ToCompare<T, TProp> toCompare,
         Func<TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (!comparer(toCompare.Actual.Value))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, null, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> IsNot<T, TProp>(
         this ToCompare<T, TProp> toCompare,
         Func<TProp, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (!comparer(toCompare.Actual.Value, toCompare.Expected))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, null, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> IsNot<T, TProp>(
         this ToCompareEnumerable<T, TProp> toCompare,
         Func<TProp, IEnumerable<TProp>, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (!comparer(toCompare.Actual.Value, toCompare.Expected))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, toCompare.Expected, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> IsNot<T, TProp>(
         this ToCompareEnumerable<T, TProp> toCompare,
         Func<IEnumerable<TProp>, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
      {
         if (!comparer(toCompare.Expected, toCompare.Actual.Value))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, toCompare.Expected, mustBeMethod);
      }

      internal static PropertyMustBe<T, TProp> IsNot<T, TProp>(
         this ToComparable<T, TProp> toCompare,
         Func<TProp, TProp, bool> comparer,
         [CallerMemberName] string mustBeMethod = null)
         where TProp : IComparable<TProp>
      {
         if (!comparer(toCompare.Actual.Value, toCompare.Expected))
         {
            return toCompare.Actual;
         }

         throw new MustBeException<T, TProp>(toCompare.Actual, toCompare.Expected, mustBeMethod);
      }

      internal static ToCompare<T, TProp> ValidateWith<T, TProp>(this PropertyMustBe<T, TProp> actual, TProp expected)
      {
         return new ToCompare<T, TProp>(actual, expected);
      }

      internal static ToCompareEnumerable<T, TProp> ValidateWith<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         IEnumerable<TProp> expected)
      {
         return new ToCompareEnumerable<T, TProp>(actual, expected);
      }

      #endregion
   }
}