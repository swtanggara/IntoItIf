namespace IntoItIf.Base.UnitOfWork
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Options;

   public interface ISaveUow : IUow
   {
      #region Public Methods and Operators

      Option<int> SaveChanges();
      Task<Option<int>> SaveChangesAsync();
      Task<Option<int>> SaveChangesAsync(Option<CancellationToken> ctok);

      #endregion
   }
}