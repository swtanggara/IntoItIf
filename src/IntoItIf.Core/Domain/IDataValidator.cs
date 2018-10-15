namespace IntoItIf.Core.Domain
{
   using System.Threading.Tasks;
   using Options;

   public interface IDataValidator<T> : IInjectable
      where T : class, IValidationEntity
   {
      #region Public Methods and Operators

      Option<ValidationResult> Validate(Option<T> toValidate);
      Task<Option<ValidationResult>> ValidateAsync(Option<T> toValidate);

      #endregion
   }
}