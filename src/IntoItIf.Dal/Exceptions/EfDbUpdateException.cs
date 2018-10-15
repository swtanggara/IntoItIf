#if NET471
namespace IntoItIf.Dal.Exceptions
{
   using System;
   using System.Data.Entity.Infrastructure;

   public class EfDbUpdateException : DbUpdateException
   {
      #region Constructors and Destructors

      public EfDbUpdateException()
      {
      }

      public EfDbUpdateException(string message) : base(message)
      {
      }

      public EfDbUpdateException(string message, Exception innerException) : base(message, innerException)
      {
      }

      #endregion
   }
}
#endif