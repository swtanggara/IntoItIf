namespace IntoItIf.Dsl.Mediator.Requests
{
   using System.Collections.Generic;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;

   public abstract class SavedRequest<T, TDto> : BaseEntityRequest<T, TDto>, IRequest<Dictionary<string, object>>
      where T : class, IEntity
      where TDto : class, IDto
   {
      #region Constructors and Destructors

      protected SavedRequest(Option<TDto> dto)
      {
         Dto = dto;
      }

      #endregion

      #region Properties

      internal Option<TDto> Dto { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Dto;
      }

      #endregion
   }
}