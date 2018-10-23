namespace IntoItIf.Base.Domain.Options
{
   using System;

   public sealed class Fail<T> : Option<T>
   {
      #region Constructors and Destructors

      private Fail(Exception exception)
      {
         Exception = exception;
      }

      #endregion

      #region Public Properties

      public Exception Exception { get; }

      #endregion

      #region Public Methods and Operators

      public static Fail<T> Throw(Exception exception)
      {
         return new Fail<T>(exception);
      }

      #endregion
   }
}