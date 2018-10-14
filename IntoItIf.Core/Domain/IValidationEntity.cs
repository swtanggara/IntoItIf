namespace IntoItIf.Core.Domain
{
   using System.Threading.Tasks;
   using Options;

   public interface IValidationEntity : IInjectable
   {
      #region Public Methods and Operators

      Option<ValidationResult> Validate(params string[] args);
      Task<Option<ValidationResult>> ValidateAsync(params string[] args);

      #endregion
   }
}