namespace IntoItIf.Base.Validations
{
   using System.Threading.Tasks;
   using Domain;

   internal sealed class EmptyDataValidator<T> : IDataValidator<T>
      where T : class, IValidationEntity
   {
      #region Public Methods and Operators

      public ValidationResult Validate(T toValidate)
      {
         return new ValidationResult(true, null);
      }

      public Task<ValidationResult> ValidateAsync(T toValidate)
      {
         return Task.FromResult(Validate(toValidate));
      }

      #endregion
   }
}