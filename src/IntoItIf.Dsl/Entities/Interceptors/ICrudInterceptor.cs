namespace IntoItIf.Dsl.Entities.Interceptors
{
   public interface ICrudInterceptor
      : ICreateInterceptor,
        IReadInterceptor,
        IUpdateInterceptor,
        IDeleteInterceptor
   {
   }
}