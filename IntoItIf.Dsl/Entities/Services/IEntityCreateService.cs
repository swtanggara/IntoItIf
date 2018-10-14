namespace IntoItIf.Dsl.Entities.Services
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;

   public interface IEntityCreateService<TDto> : IEntityService
      where TDto : class, IDto
   {
      #region Public Methods and Operators

      Option<Dictionary<string, object>> CreateEntity(Option<TDto> dto);

      Task<Option<Dictionary<string, object>>> CreateEntityAsync(Option<TDto> dto, Option<CancellationToken> ctok);

      #endregion
   }
}