namespace IntoItIf.Core.Exceptions
{
   using System;
   using System.Collections.Generic;

   public class ValidationException : Exception
   {
      #region Constructors and Destructors

      public ValidationException(IEnumerable<string> messages) : base(string.Join("\n", messages))
      {
         Messages = messages;
      }

      #endregion

      #region Public Properties

      public IEnumerable<string> Messages { get; }

      #endregion
   }
}