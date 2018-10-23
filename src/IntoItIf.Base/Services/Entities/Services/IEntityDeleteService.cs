namespace IntoItIf.Base.Services.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Entities;
   using Domain.Options;

   public interface IEntityDeleteService<TDto> : IEntityService
      where TDto : class, IDto
   {
      #region Public Methods and Operators

      Option<bool> DeleteEntity(Option<TDto> criteria);
      Task<Option<bool>> DeleteEntityAsync(Option<TDto> criteria, Option<CancellationToken> ctok);

      #endregion
   }
}