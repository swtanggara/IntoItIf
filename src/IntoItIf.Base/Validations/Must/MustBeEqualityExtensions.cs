namespace IntoItIf.Base.Validations.Must
{
   using System;
   using Internals;

   public static class MustBeEqualityExtensions
   {
      #region Public Methods and Operators

      public static PropertyMustBe<T, TProp> MustBe<T, TProp>(this PropertyMustBe<T, TProp> actual, TProp expected)
      {
         return actual.ValidateWith(expected).Is(Is.Equal);
      }

      public static PropertyMustBe<T, bool> MustBeFalse<T>(this PropertyMustBe<T, bool> actual)
      {
         return actual.ValidateWith(false).Is(Is.Equal);
      }

      public static PropertyMustBe<T, bool?> MustBeFalse<T>(this PropertyMustBe<T, bool?> actual)
      {
         return actual.ValidateWith(false).Is(Is.Equal);
      }

      public static PropertyMustBe<T, TProp> MustBeGreaterThan<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         TProp expected)
         where TProp : IComparable<TProp>
      {
         return actual.CompareTo(expected).Is(Is.GreaterThan);
      }

      public static PropertyMustBe<T, TProp> MustBeGreaterThanOrEqualTo<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         TProp expected)
         where TProp : IComparable<TProp>
      {
         return actual.CompareTo(expected).Is(Is.GreaterThanOrEqualTo);
      }

      public static PropertyMustBe<T, TProp> MustBeInRange<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         TProp from,
         TProp to)
         where TProp : IComparable<TProp>
      {
         return actual.RangedWith(from, to).Is(Is.InRange);
      }

      public static PropertyMustBe<T, TProp> MustBeLessThan<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         TProp expected)
         where TProp : IComparable<TProp>
      {
         return actual.CompareTo(expected).Is(Is.LessThan);
      }

      public static PropertyMustBe<T, TProp> MustBeLessThanOrEqualTo<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         TProp expected)
         where TProp : IComparable<TProp>
      {
         return actual.CompareTo(expected).Is(Is.LessThanOrEqualTo);
      }

      public static PropertyMustBe<T, byte> MustBeNegative<T>(this PropertyMustBe<T, byte> actual)
      {
         return MustBeNegative<T, byte>(actual);
      }

      public static PropertyMustBe<T, short> MustBeNegative<T>(this PropertyMustBe<T, short> actual)
      {
         return MustBeNegative<T, short>(actual);
      }

      public static PropertyMustBe<T, int> MustBeNegative<T>(this PropertyMustBe<T, int> actual)
      {
         return MustBeNegative<T, int>(actual);
      }

      public static PropertyMustBe<T, long> MustBeNegative<T>(this PropertyMustBe<T, long> actual)
      {
         return MustBeNegative<T, long>(actual);
      }

      public static PropertyMustBe<T, float> MustBeNegative<T>(this PropertyMustBe<T, float> actual)
      {
         return MustBeNegative<T, float>(actual);
      }

      public static PropertyMustBe<T, double> MustBeNegative<T>(this PropertyMustBe<T, double> actual)
      {
         return MustBeNegative<T, double>(actual);
      }

      public static PropertyMustBe<T, decimal> MustBeNegative<T>(this PropertyMustBe<T, decimal> actual)
      {
         return MustBeNegative<T, decimal>(actual);
      }

      public static PropertyMustBe<T, byte> MustBePositive<T>(this PropertyMustBe<T, byte> actual)
      {
         return MustBePositive<T, byte>(actual);
      }

      public static PropertyMustBe<T, short> MustBePositive<T>(this PropertyMustBe<T, short> actual)
      {
         return MustBePositive<T, short>(actual);
      }

      public static PropertyMustBe<T, int> MustBePositive<T>(this PropertyMustBe<T, int> actual)
      {
         return MustBePositive<T, int>(actual);
      }

      public static PropertyMustBe<T, long> MustBePositive<T>(this PropertyMustBe<T, long> actual)
      {
         return MustBePositive<T, long>(actual);
      }

      public static PropertyMustBe<T, float> MustBePositive<T>(this PropertyMustBe<T, float> actual)
      {
         return MustBePositive<T, float>(actual);
      }

      public static PropertyMustBe<T, double> MustBePositive<T>(this PropertyMustBe<T, double> actual)
      {
         return MustBePositive<T, double>(actual);
      }

      public static PropertyMustBe<T, decimal> MustBePositive<T>(this PropertyMustBe<T, decimal> actual)
      {
         return MustBePositive<T, decimal>(actual);
      }

      public static PropertyMustBe<T, bool> MustBeTrue<T>(this PropertyMustBe<T, bool> actual)
      {
         return actual.ValidateWith(true).Is(Is.Equal);
      }

      public static PropertyMustBe<T, bool?> MustBeTrue<T>(this PropertyMustBe<T, bool?> actual)
      {
         return actual.ValidateWith(true).Is(Is.Equal);
      }

      public static PropertyMustBe<T, TProp> MustNotBe<T, TProp>(this PropertyMustBe<T, TProp> actual, TProp expected)
      {
         return actual.ValidateWith(expected).IsNot(Is.Equal);
      }

      public static PropertyMustBe<T, TProp> MustNotBeInRange<T, TProp>(
         this PropertyMustBe<T, TProp> actual,
         TProp from,
         TProp to)
         where TProp : IComparable<TProp>
      {
         return actual.RangedWith(from, to).IsNot(Is.InRange);
      }

      #endregion

      #region Methods

      private static PropertyMustBe<T, TValue> MustBeNegative<T, TValue>(this PropertyMustBe<T, TValue> actual)
         where TValue : IComparable<TValue>
      {
         var expected = default(TValue);
         return actual.CompareTo(expected).Is(Is.LessThan);
      }

      private static PropertyMustBe<T, TValue> MustBePositive<T, TValue>(this PropertyMustBe<T, TValue> actual)
         where TValue : IComparable<TValue>
      {
         var expected = default(TValue);
         return actual.CompareTo(expected).Is(Is.GreaterThan);
      }

      #endregion
   }
}