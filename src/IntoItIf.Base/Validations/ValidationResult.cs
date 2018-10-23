namespace IntoItIf.Base.Validations
{
   using System.Collections.Generic;

   public sealed class ValidationResult
   {
      #region Constructors and Destructors

      public ValidationResult(bool isValid, params string[] errorMessages)
      {
         IsValid = isValid;
         ErrorMessages = errorMessages;
      }

      #endregion

      #region Public Properties

      public bool IsValid { get; }
      public IEnumerable<string> ErrorMessages { get; }

      #endregion
   }
}