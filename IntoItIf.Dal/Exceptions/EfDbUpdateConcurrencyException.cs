#if NET471
namespace IntoItIf.Dal.Exceptions
{
   using System;
   using System.Data.Entity.Infrastructure;

   public class EfDbUpdateConcurrencyException : DbUpdateConcurrencyException
   {
      #region Constructors and Destructors

      public EfDbUpdateConcurrencyException(string message) : base(message)
      {
      }

      public EfDbUpdateConcurrencyException(string message, Exception innerException) : base(message, innerException)
      {
      }

      #endregion
   }
}
#endif