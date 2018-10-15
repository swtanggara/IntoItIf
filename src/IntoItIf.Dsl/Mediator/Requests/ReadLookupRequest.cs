namespace IntoItIf.Dsl.Mediator.Requests
{
   using System.Collections.Generic;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Mediator;

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