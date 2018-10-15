namespace IntoItIf.Tests
{
   using Dsl.Validators;
   using FluentValidation;

   public class MyDtoFluentValidator : BaseFluentValidator<MyDto>
   {
      public MyDtoFluentValidator()
      {
         RuleFor(x => x.Id).NotEmpty();
      }
   }
}