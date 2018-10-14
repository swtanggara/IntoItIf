namespace IntoItIf.Dsl.Entities.Interceptors
{
   using Args;

   public interface IReadOneInterceptor : IInterceptor
   {
      #region Public Methods and Operators

      ReadOneInterceptorArgs<T> OpenForReadOne<T>();

      #endregion
   }
}