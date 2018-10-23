namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System.Collections.Generic;
   using Base.Mediator;
   using Domain;
   using Domain.Entities;
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