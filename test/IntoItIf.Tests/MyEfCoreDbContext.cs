namespace IntoItIf.Tests
{
   using Dal.DbContexts;
   using Microsoft.EntityFrameworkCore;

   public class MyEfCoreDbContext : EfCoreDbContext
   {
      #region Constructors and Destructors

      public MyEfCoreDbContext() : this(null)
      {
      }

      public MyEfCoreDbContext(DbContextOptions options) : base(options)
      {
      }

      #endregion
   }
}
