namespace IntoItIf.Dsl.Exceptions
{
   using System;

   public class PreValidationException : Exception
   {
      #region Constructors and Destructors

      public PreValidationException(string message) : base(message)
      {
      }

      #endregion
   }
}