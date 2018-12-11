namespace IntoItIf.Base.Validations
{
   using System.Threading.Tasks;
   using Domain;
   using Services;

   public interface IDataValidator<in T> : IInjectable
      where T : class, IValidationEntity
   {
      #region Public Methods and Operators

      ValidationResult Validate(T toValidate);
      Task<ValidationResult> ValidateAsync(T toValidate);

      #endregion
   }
}