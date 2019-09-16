namespace IntoItIf.Base.Validations.Must
{
   public enum MustBeMethod
   {
      Unspecified = 0,

      MustBe,
      MustNotBe,

      MustBeNull,
      MustNotBeNull,

      MustBeOneOf,
      MustNotBeOneOf,

      MustBeNullOrWhiteSpace,
      MustNotBeNullOrWhiteSpace,
      MustBeNullOrEmpty,
      MustNotBeNullOrEmpty,

      MustBePositive,
      MustBeNegative,

      MustBeInRange,
   }
}