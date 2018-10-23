namespace IntoItIf.Base.Services.Mediator.Requests
{
   using System.Collections.Generic;
   using Base.Mediator;
   using Domain.Entities;
   using Domain.Options;

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