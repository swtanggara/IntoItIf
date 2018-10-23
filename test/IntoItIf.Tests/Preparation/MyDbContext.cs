namespace IntoItIf.Tests.Preparation
{
   using Ef.DbContexts;
   using Microsoft.EntityFrameworkCore;

   public class MyDbContext : ItsDbContext
   {
      #region Constructors and Destructors

      public MyDbContext() : this(null)
      {
      }

      public MyDbContext(DbContextOptions options) : base(options)
      {
      }

      #endregion
   }
}
