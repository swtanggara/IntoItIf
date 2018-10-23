namespace IntoItIf.Base.Services.Entities.Interceptors
{
   using Args;

   public interface IReadLookupInterceptor : IInterceptor
   {
      #region Public Methods and Operators

      ReadLookupInterceptorArgs<T> OpenForReadLookup<T>();

      #endregion
   }
}