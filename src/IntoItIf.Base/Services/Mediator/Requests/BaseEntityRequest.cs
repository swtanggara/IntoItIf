namespace IntoItIf.Base.Services.Mediator.Requests
{
   using Domain;
   using Domain.Entities;

   public abstract class BaseEntityRequest<T, TDto> : ValueObject<BaseEntityRequest<T, TDto>>
      where T : class, IEntity
      where TDto : class, IDto
   {
   }
}