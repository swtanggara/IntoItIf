namespace IntoItIf.Dsl.Entities.Interceptors
{
   using Args;

   public interface IReadPagedInterceptor : IInterceptor
   {
      #region Public Methods and Operators

      ReadPagedInterceptorArgs<T> OpenForReadPaged<T>();

      #endregion
   }
}