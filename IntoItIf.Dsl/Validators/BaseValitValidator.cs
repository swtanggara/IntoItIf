namespace IntoItIf.Dsl.Validators
{
   using System.Linq;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Options;
   using Valit;

   public abstract class BaseValitValidator<T> : IDataValidator<T>
      where T : class, IValidationEntity
   {
      #region Properties

      protected abstract ValitRules<T> ValitRules { get; }

      #endregion

      #region Public Methods and Operators

      public Option<ValidationResult> Validate(Option<T> toValidate)
      {
         return toValidate.Map(
            x =>
            {
               var result = ValitRules<T>.Create(ValitRules).For(x).Validate();
               return new ValidationResult(result.Succeeded, result.ErrorMessages.ToArray());
            });
      }

      public Task<Option<ValidationResult>> ValidateAsync(Option<T> toValidate)
      {
         return Task.FromResult(Validate(toValidate));
      }

      #endregion
   }
}