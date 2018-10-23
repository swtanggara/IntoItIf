namespace IntoItIf.Base.Validations
{
   using System.Threading.Tasks;
   using Domain;
   using Domain.Options;

   internal sealed class EmptyDataValidator<T> : IDataValidator<T>
      where T : class, IValidationEntity
   {
      #region Public Methods and Operators

      public Option<ValidationResult> Validate(Option<T> toValidate)
      {
         return new ValidationResult(true, null);
      }

      public Task<Option<ValidationResult>> ValidateAsync(Option<T> toValidate)
      {
         return Validate(toValidate).GetOptionTask();
      }

      #endregion
   }
}