namespace IntoItIf.Base.Services.Entities.Interceptors
{
   using Args;

   public interface ICreateInterceptor : IInterceptor
   {
      #region Public Methods and Operators

      SaveInterceptorArgs<T, TDto> OpenForCreate<T, TDto>();

      #endregion
   }
}