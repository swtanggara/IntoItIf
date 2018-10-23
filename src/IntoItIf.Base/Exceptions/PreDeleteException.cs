namespace IntoItIf.Base.Exceptions
{
   using System;

   public class PreDeleteException : Exception
   {
      #region Constructors and Destructors

      public PreDeleteException(string message) : base(message)
      {
      }

      #endregion
   }
}