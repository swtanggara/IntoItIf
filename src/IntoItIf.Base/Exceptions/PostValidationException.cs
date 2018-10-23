namespace IntoItIf.Base.Exceptions
{
   using System;

   public class PostValidationException : Exception
   {
      #region Constructors and Destructors

      public PostValidationException(string message) : base(message)
      {
      }

      #endregion
   }
}