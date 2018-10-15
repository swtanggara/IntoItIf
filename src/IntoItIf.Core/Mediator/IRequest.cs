namespace IntoItIf.Core.Mediator
{
   public interface IRequest<TResponse> : IBaseRequest
   {
   }

   public interface IRequest : IRequest<bool>
   {
   }
}