namespace IntoItIf.Base.Services.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Entities;
   using Domain.Options;

   public interface IEntityReadPagedService<TDto>
      where TDto : class, IDto
   {
      #region Public Methods and Operators

      Option<IPaged<TDto>> GetPaged(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword);

      Task<Option<IPaged<TDto>>> GetPagedAsync(
         Option<int> pageNo,
         Option<int> pageSize,
         Option<string>[] sorts,
         Option<string> keyword,
         Option<CancellationToken> ctok);

      #endregion
   }
}