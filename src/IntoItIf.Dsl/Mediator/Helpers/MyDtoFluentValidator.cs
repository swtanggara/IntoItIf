namespace IntoItIf.Dsl.Mediator.Helpers
{
   using FluentValidation;
   using Validators;

   public class MyDtoFluentValidator : BaseFluentValidator<MyDto>
   {
      public MyDtoFluentValidator()
      {
         RuleFor(x => x.Id).NotEmpty();
      }
   }
}