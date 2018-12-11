namespace IntoItIf.Base.UnitOfWork
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Options;

   public interface ISaveUow : IUow
   {
      #region Public Methods and Operators

      int SaveChanges();
      Task<int> SaveChangesAsync();
      Task<int> SaveChangesAsync(CancellationToken ctok);

      #endregion
   }
}