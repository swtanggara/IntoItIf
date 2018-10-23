namespace IntoItIf.Base.Validations
{
   using System.Linq;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Options;
   using FluentValidation;

   public abstract class BaseFluentValidator<T> : AbstractValidator<T>, IDataValidator<T>
      where T : class, IValidationEntity
   {
      #region Public Methods and Operators

      public Option<ValidationResult> Validate(Option<T> toValidate)
      {
         return toValidate.Map(
            x =>
            {
               var result = base.Validate(x);
               return new ValidationResult(result.IsValid, result.Errors.Select(y => y.ErrorMessage).ToArray());
            });
      }

      public Task<Option<ValidationResult>> ValidateAsync(Option<T> toValidate)
      {
         return toValidate.MapAsync(
            async x =>
            {
               var result = await base.ValidateAsync(x);
               return new ValidationResult(result.IsValid, result.Errors.Select(y => y.ErrorMessage).ToArray());
            });
      }

      #endregion
   }
}