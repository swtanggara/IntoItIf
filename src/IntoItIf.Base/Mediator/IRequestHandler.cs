namespace IntoItIf.Base.Mediator
{
   using System.Threading;
   using System.Threading.Tasks;

   public interface IRequestHandler<in TRequest, TResponse>
      where TRequest : IRequest<TResponse>
   {
      #region Public Methods and Operators

      Task<TResponse> HandleAsync(TRequest request, CancellationToken ctok);

      #endregion
   }

   public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, bool>
      where TRequest : IRequest
   {
   }
}