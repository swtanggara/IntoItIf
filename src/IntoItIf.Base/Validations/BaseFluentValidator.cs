namespace IntoItIf.Base.Validations
{
   using System.Linq;
   using System.Threading.Tasks;
   using Domain;
   using FluentValidation;

   public abstract class BaseFluentValidator<T> : AbstractValidator<T>, IDataValidator<T>
      where T : class, IValidationEntity
   {
      #region Public Methods and Operators

      ValidationResult IDataValidator<T>.Validate(T toValidate)
      {
         var result = Validate(toValidate);
         return new ValidationResult(result.IsValid, result.Errors.Select(y => y.ErrorMessage).ToArray());
      }

      public async Task<ValidationResult> ValidateAsync(T toValidate)
      {
         var result = await base.ValidateAsync(toValidate);
         return new ValidationResult(result.IsValid, result.Errors.Select(y => y.ErrorMessage).ToArray());
      }

      #endregion
   }
}