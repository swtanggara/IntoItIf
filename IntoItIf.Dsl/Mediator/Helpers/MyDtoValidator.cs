namespace IntoItIf.Dsl.Mediator.Helpers
{
   using Validators;
   using Valit;

   public class MyDtoValidator : BaseValitValidator<MyDto>
   {
      #region Properties

      protected override ValitRules<MyDto> ValitRules { get; }

      #endregion
   }
}