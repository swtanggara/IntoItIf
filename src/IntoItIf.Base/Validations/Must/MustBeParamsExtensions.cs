namespace IntoItIf.Base.Validations.Must
{
   using System.Linq;

   public static class MustBeParamsExtensions
   {
      #region Public Methods and Operators

      public static PropertyMustBe<T, TProp> MustBeOneOf<T, TProp>(this PropertyMustBe<T, TProp> actual, params TProp[] expected)
      {
         return actual.ValidateWith(expected).Is(Enumerable.Contains);
      }

      public static PropertyMustBe<T, TProp> MustNotBeOneOf<T, TProp>(this PropertyMustBe<T, TProp> actual, params TProp[] expected)
      {
         return actual.ValidateWith(expected).IsNot(Enumerable.Contains);
      }

      #endregion
   }
}