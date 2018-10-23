namespace IntoItIf.Base.Services.Mediator.Requests
{
   using System.Collections.Generic;
   using Base.Mediator;
   using Domain;
   using Domain.Entities;
   using Domain.Options;

   public sealed class ReadLookupRequest<T, TDto> : BaseEntityRequest<T, TDto>, IRequest<List<KeyValue>>
      where T : class, IEntity where TDto : class, IDto
   {
      #region Constructors and Destructors

      public ReadLookupRequest(Option<bool> useValueAsId)
      {
         UseValueAsId = useValueAsId;
      }

      #endregion

      #region Properties

      internal Option<bool> UseValueAsId { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return UseValueAsId;
      }

      #endregion
   }
}