namespace IntoItIf.Tests.Preparation
{
   using Base.Validations;
   using FluentValidation;

   public class MyDtoFluentValidator : BaseFluentValidator<MyDto>
   {
      public MyDtoFluentValidator()
      {
         RuleFor(x => x.Id).NotEmpty();
      }
   }
}