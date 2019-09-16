namespace IntoItIf.Base.Validations.Must
{
   using System.Collections.Generic;
   using System.Linq;

   public static class MustBeEnumerableExtensions
   {
      public static PropertyMustBe<T, IList<TProp>> MustBeEmpty<T, TProp>(this PropertyMustBe<T, IList<TProp>> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x == null || !x.Any());
      }

      public static PropertyMustBe<T, IList<TProp>> MustNotBeEmpty<T, TProp>(this PropertyMustBe<T, IList<TProp>> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x != null && x.Any());
      }

      public static PropertyMustBe<T, IEnumerable<TProp>> MustBeEmpty<T, TProp>(this PropertyMustBe<T, IEnumerable<TProp>> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x == null || !x.Any());
      }

      public static PropertyMustBe<T, IEnumerable<TProp>> MustNotBeEmpty<T, TProp>(this PropertyMustBe<T, IEnumerable<TProp>> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x != null && x.Any());
      }

      public static PropertyMustBe<T, IReadOnlyList<TProp>> MustBeEmpty<T, TProp>(this PropertyMustBe<T, IReadOnlyList<TProp>> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x == null || !x.Any());
      }

      public static PropertyMustBe<T, IReadOnlyList<TProp>> MustNotBeEmpty<T, TProp>(this PropertyMustBe<T, IReadOnlyList<TProp>> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x != null && x.Any());
      }

      public static PropertyMustBe<T, TProp[]> MustBeEmpty<T, TProp>(this PropertyMustBe<T, TProp[]> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x == null || !x.Any());
      }

      public static PropertyMustBe<T, TProp[]> MustNotBeEmpty<T, TProp>(this PropertyMustBe<T, TProp[]> actual)
      {
         return actual.ValidateWith(actual.Value).Is(x => x != null && x.Any());
      }
   }
}