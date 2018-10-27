namespace IntoItIf.Base.Validations
{
   using System.Collections.Generic;
   using Domain.Options;
   using Exceptions;

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

      public Fail<T> ToFail<T>()
      {
         return Fail<T>.Throw(new ValidationException(ErrorMessages));
      }

      #endregion
   }
}