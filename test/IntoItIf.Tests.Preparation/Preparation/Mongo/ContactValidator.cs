namespace IntoItIf.Tests.Preparation.Preparation.Mongo
{
   using Base.Validations;
   using FluentValidation;

   public class ContactValidator : BaseFluentValidator<Contact>
   {
      #region Constructors and Destructors

      public ContactValidator()
      {
         RuleFor(x => x.FirstName).NotEmpty();
         RuleFor(x => x.LastName).NotEmpty();
         RuleFor(x => x.Age).GreaterThan(0);
      }

      #endregion
   }
}