namespace IntoItIf.Dsl.Mediator.Handlers
{
   using System.Collections.Generic;
   using Core.Domain.Entities;
   using Core.Mediator;
   using Requests;

   public interface ICrudHandler<T, TDto>
      : IRequestHandler<CreateRequest<T, TDto>, Dictionary<string, object>>,
        IReadHandler<T, TDto>,
        IRequestHandler<UpdateRequest<T, TDto>, Dictionary<string, object>>,
        IRequestHandler<DeleteRequest<T, TDto>>
      where T : class, IEntity
      where TDto : class, IDto
   {
   }
}