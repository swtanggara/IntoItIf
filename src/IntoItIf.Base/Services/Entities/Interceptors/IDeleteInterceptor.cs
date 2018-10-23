namespace IntoItIf.Base.Services.Entities.Interceptors
{
   using Args;

   public interface IDeleteInterceptor : IInterceptor
   {
      #region Public Methods and Operators

      DeleteInterceptorArgs<T> OpenForDelete<T>();

      #endregion
   }
}