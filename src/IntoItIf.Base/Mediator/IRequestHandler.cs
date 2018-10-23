namespace IntoItIf.Base.Mediator
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Options;

   public interface IRequestHandler<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
   {
      #region Public Methods and Operators

      Task<Option<TResponse>> Handle(Option<TRequest> request, Option<CancellationToken> ctok);

      #endregion
   }

   public interface IRequestHandler<TRequest> : IRequestHandler<TRequest, bool>
      where TRequest : IRequest
   {
   }
}