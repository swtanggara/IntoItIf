namespace IntoItIf.Base.Services.Mediator.Requests
{
   using Domain.Entities;
   using Domain.Options;

   public sealed class CreateRequest<T, TDto> : SavedRequest<T, TDto>
      where T : class, IEntity where TDto : class, IDto
   {
      #region Constructors and Destructors

      public CreateRequest(Option<TDto> dto) : base(dto)
      {
      }

      #endregion
   }
}