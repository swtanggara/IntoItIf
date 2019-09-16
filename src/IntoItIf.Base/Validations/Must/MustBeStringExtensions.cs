namespace IntoItIf.Base.Validations.Must
{
   using System;
   using Internals;

   public static class MustBeStringExtensions
   {
      #region Public Methods and Operators

      public static PropertyMustBe<T, string> MustBe<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).Is(EqualSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).Is(EqualInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      public static PropertyMustBe<T, string> MustBe<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustBe(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustContains<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustContains(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustContains<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).Is(ContainsSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).Is(ContainsInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      public static PropertyMustBe<T, string> MustEndsWith<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustEndsWith(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustEndsWith<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).Is(EndsWithSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).Is(EndsWithInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      public static PropertyMustBe<T, string> MustBeNullOrEmpty<T>(this PropertyMustBe<T, string> actual)
      {
         return actual.Is(string.IsNullOrEmpty);
      }

      public static PropertyMustBe<T, string> MustBeNullOrWhiteSpace<T>(this PropertyMustBe<T, string> actual)
      {
         return actual.Is(string.IsNullOrWhiteSpace);
      }

      public static PropertyMustBe<T, string> MustStartsWith<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustStartsWith(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustStartsWith<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).Is(StartsWithSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).Is(StartsWithInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      public static PropertyMustBe<T, string> MustNotBe<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustNotBe(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustNotBe<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).IsNot(EqualSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).IsNot(EqualInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      public static PropertyMustBe<T, string> MustNotContains<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustNotContains(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustNotContains<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).IsNot(ContainsSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).IsNot(ContainsInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      public static PropertyMustBe<T, string> MustNotEndsWith<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustNotEndsWith(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustNotEndsWith<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).IsNot(EndsWithSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).IsNot(EndsWithInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      public static PropertyMustBe<T, string> MustNotBeNullOrEmpty<T>(this PropertyMustBe<T, string> actual)
      {
         return actual.IsNot(string.IsNullOrEmpty);
      }

      public static PropertyMustBe<T, string> MustNotBeNullOrWhiteSpace<T>(this PropertyMustBe<T, string> actual)
      {
         return actual.IsNot(string.IsNullOrWhiteSpace);
      }

      public static PropertyMustBe<T, string> MustNotStartsWith<T>(this PropertyMustBe<T, string> actual, string expected)
      {
         return MustNotStartsWith(actual, expected, Case.Insensitive);
      }

      public static PropertyMustBe<T, string> MustNotStartsWith<T>(this PropertyMustBe<T, string> actual, string expected, Case @case)
      {
         switch (@case)
         {
            case Case.Sensitive:
               return actual.ValidateWith(expected).IsNot(StartsWithSensitive);
            case Case.Insensitive:
               return actual.ValidateWith(expected).IsNot(StartsWithInsensitive);
            default:
               throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
         }
      }

      #endregion

      #region Methods

      private static bool ContainsInsensitive(string x, string y)
      {
         return x.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0;
      }

      private static bool ContainsSensitive(string x, string y)
      {
         return x.IndexOf(y, StringComparison.Ordinal) >= 0;
      }

      private static bool EndsWithInsensitive(string x, string y)
      {
         return x.EndsWith(y, StringComparison.OrdinalIgnoreCase);
      }

      private static bool EndsWithSensitive(string x, string y)
      {
         return x.EndsWith(y, StringComparison.Ordinal);
      }

      private static bool EqualInsensitive(string x, string y)
      {
         return StringComparer.OrdinalIgnoreCase.Equals(x, y);
      }

      private static bool EqualSensitive(string x, string y)
      {
         return StringComparer.Ordinal.Equals(x, y);
      }

      private static bool StartsWithInsensitive(string x, string y)
      {
         return x.StartsWith(y, StringComparison.OrdinalIgnoreCase);
      }

      private static bool StartsWithSensitive(string x, string y)
      {
         return x.StartsWith(y, StringComparison.Ordinal);
      }

      #endregion
   }
}