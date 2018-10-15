namespace IntoItIf.Dsl.Mediator.Requests
{
   using System.Collections.Generic;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;

   public sealed class ReadOneRequest<T, TDto> : BaseEntityRequest<T, TDto>, IRequest<TDto>
      where T : class, IEntity where TDto : class, IDto
   {
      #region Constructors and Destructors

      public ReadOneRequest(Option<TDto> criteria)
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