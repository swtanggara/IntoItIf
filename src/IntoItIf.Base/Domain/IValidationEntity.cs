namespace IntoItIf.Base.Domain
{
   using System.Threading.Tasks;
   using Options;
   using Services;
   using Validations;

   public interface IValidationEntity : IInjectable
   {
      #region Public Methods and Operators

      Option<ValidationResult> Validate(params string[] args);
      Task<Option<ValidationResult>> ValidateAsync(params string[] args);

      #endregion
   }
}