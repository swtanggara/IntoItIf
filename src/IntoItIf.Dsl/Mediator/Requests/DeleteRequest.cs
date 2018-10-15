namespace IntoItIf.Dsl.Mediator.Requests
{
   using System.Collections.Generic;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;

   public sealed class DeleteRequest<T, TDto> : BaseEntityRequest<T, TDto>, IRequest
      where T : class, IEntity where TDto : class, IDto
   {
      #region Constructors and Destructors

      public DeleteRequest(Option<TDto> criteria)
      {
         Criteria = criteria;
      }

      #endregion

      #region Properties

      internal Option<TDto> Criteria { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Criteria;
      }

      #endregion
   }
}