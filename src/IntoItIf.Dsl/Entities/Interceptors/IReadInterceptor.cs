namespace IntoItIf.Dsl.Entities.Interceptors
{
   public interface IReadInterceptor
      : IReadOneInterceptor,
        IReadLookupInterceptor,
        IReadPagedInterceptor
   {
   }
}