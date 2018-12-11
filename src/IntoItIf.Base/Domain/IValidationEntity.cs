namespace IntoItIf.Base.Domain
{
   using System.Threading.Tasks;
   using Services;
   using Validations;

   public interface IValidationEntity : IInjectable
   {
      #region Public Methods and Operators

      ValidationResult Validate(params string[] args);
      Task<ValidationResult> ValidateAsync(params string[] args);

      #endregion
   }
}