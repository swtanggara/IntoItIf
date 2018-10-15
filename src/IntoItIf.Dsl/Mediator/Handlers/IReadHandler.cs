namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System.Collections.Generic;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Mediator;
   using Requests;

   public interface IReadHandler<T, TDto>
      : IRequestHandler<ReadLookupRequest<T, TDto>, List<KeyValue>>,
        IRequestHandler<ReadOneRequest<T, TDto>, TDto>,
        IRequestHandler<ReadPagedRequest<T, TDto>, IPaged<TDto>>
      where T : class, IEntity
      where TDto : class, IDto
   {
   }
}