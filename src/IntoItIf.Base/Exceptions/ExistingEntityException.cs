namespace IntoItIf.Base.Exceptions
{
   using System;

   public class ExistingEntityException : Exception
   {
      #region Constructors and Destructors

      public ExistingEntityException(string message)
      {
         Message = message;
      }

      #endregion

      #region Public Properties

      public override string Message { get; }

      #endregion
   }

   public class ExistingEntityException<T> : ExistingEntityException
   {
      #region Constructors and Destructors

      public ExistingEntityException(Func<T, string> existMessageFunc, T exist) : base(existMessageFunc(exist))
      {
      }

      public ExistingEntityException(Func<T, string[], string> existMessageFunc, T exist, string[] propertyNames)
         : base(existMessageFunc(exist, propertyNames))
      {
      }

      #endregion
   }
}