#if NETSTANDARD2_0
namespace IntoItIf.Dal.Exceptions
{
   using System.Collections.Generic;
   using System.Linq;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.ChangeTracking;
   using Microsoft.EntityFrameworkCore.Update;

   public class EfCoreDbUpdateConcurrencyException : DbUpdateConcurrencyException
   {
      #region Constructors and Destructors

      public EfCoreDbUpdateConcurrencyException(string message, IReadOnlyList<IUpdateEntry> entries) : base(
         message,
         entries)
      {
      }

      // ReSharper disable once SuspiciousTypeConversion.Global
      public EfCoreDbUpdateConcurrencyException(string message, IReadOnlyList<EntityEntry> entries) : base(
         message,
         entries.Select(x => (IUpdateEntry)x).ToList())
      {
      }

      #endregion
   }
}
#endif