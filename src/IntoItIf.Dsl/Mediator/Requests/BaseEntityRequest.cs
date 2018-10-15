namespace IntoItIf.Dsl.Mediator.Requests
{
   using Core.Domain;
   using Core.Domain.Entities;

   public abstract class BaseEntityRequest<T, TDto> : ValueObject<BaseEntityRequest<T, TDto>>
      where T : class, IEntity
      where TDto : class, IDto
   {
   }
}