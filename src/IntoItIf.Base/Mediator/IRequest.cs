namespace IntoItIf.Base.Mediator
{
   // ReSharper disable once UnusedTypeParameter
   public interface IRequest<TResponse>
   {
   }

   public interface IRequest : IRequest<bool>
   {
   }
}