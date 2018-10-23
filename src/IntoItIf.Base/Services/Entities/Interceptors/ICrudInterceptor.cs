namespace IntoItIf.Base.Services.Entities.Interceptors
{
   public interface ICrudInterceptor
      : ICreateInterceptor,
        IReadInterceptor,
        IUpdateInterceptor,
        IDeleteInterceptor
   {
   }
}