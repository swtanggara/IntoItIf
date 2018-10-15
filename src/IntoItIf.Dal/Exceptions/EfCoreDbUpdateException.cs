#if NETSTANDARD2_0
namespace IntoItIf.Dal.Exceptions
{
   using System;
   using System.Collections.Generic;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.Update;

   public class EfCoreDbUpdateException : DbUpdateException
   {
      #region Constructors and Destructors

      public EfCoreDbUpdateException(string message, Exception innerException) : base(message, innerException)
      {
      }

      public EfCoreDbUpdateException(string message, IReadOnlyList<IUpdateEntry> entries) : base(message, entries)
      {
      }

      public EfCoreDbUpdateException(string message, Exception innerException, IReadOnlyList<IUpdateEntry> entries) :
         base(message, innerException, entries)
      {
      }

      #endregion
   }
}
#endif