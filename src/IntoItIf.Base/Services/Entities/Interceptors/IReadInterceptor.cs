namespace IntoItIf.Base.Services.Entities.Interceptors
{
   public interface IReadInterceptor
      : IReadOneInterceptor,
        IReadLookupInterceptor,
        IReadPagedInterceptor
   {
   }
}