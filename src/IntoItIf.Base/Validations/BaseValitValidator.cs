namespace IntoItIf.Base.Validations
{
   using System.Linq;
   using System.Threading.Tasks;
   using Domain;
   using Valit;

   public abstract class BaseValitValidator<T> : IDataValidator<T>
      where T : class, IValidationEntity
   {
      #region Properties

      protected abstract IValitator<T> Valitator { get; }

      #endregion

      #region Public Methods and Operators

      public ValidationResult Validate(T toValidate)
      {
         var result = Valitator.Validate(toValidate);
         return new ValidationResult(result.Succeeded, result.ErrorMessages.ToArray());
      }

      public Task<ValidationResult> ValidateAsync(T toValidate)
      {
         return Task.FromResult(Validate(toValidate));
      }

      #endregion
   }
}