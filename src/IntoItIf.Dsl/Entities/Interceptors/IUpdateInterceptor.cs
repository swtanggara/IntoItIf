namespace IntoItIf.Dsl.Entities.Interceptors
{
   using Args;

   public interface IUpdateInterceptor : IInterceptor
   {
      #region Public Methods and Operators

      SaveInterceptorArgs<T, TDto> OpenForUpdate<T, TDto>();

      #endregion
   }
}