namespace IntoItIf.Base.Validations.Must
{
   public static class MustBeNullExtensions
   {
      #region Public Methods and Operators

      public static PropertyMustBe<T, TProp> MustBeNull<T, TProp>(this PropertyMustBe<T, TProp> actual)
      {
         return actual.Is(x => x == null);
      }

      public static PropertyMustBe<T, TProp> MustNotBeNull<T, TProp>(this PropertyMustBe<T, TProp> actual)
      {
         return actual.IsNot(x => x == null);
      }

      #endregion
   }
}