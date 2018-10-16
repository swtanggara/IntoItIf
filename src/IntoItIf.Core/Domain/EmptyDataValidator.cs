namespace IntoItIf.Core.Domain
{
   using System.Threading.Tasks;
   using Options;

   internal sealed class EmptyDataValidator<T> : IDataValidator<T>
      where T : class, IValidationEntity
   {
      public Option<ValidationResult> Validate(Option<T> toValidate)
      {
         return new ValidationResult(true, null);
      }

      public Task<Option<ValidationResult>> ValidateAsync(Option<T> toValidate)
      {
         return Task.FromResult(new ValidationResult(true, null).ToOption());
      }
   }
}