namespace IntoItIf.Base.Services.Mediator.Handlers
{
   using System.Collections.Generic;
   using Base.Mediator;
   using Domain.Entities;
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