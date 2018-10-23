namespace IntoItIf.Base.Services.Entities.Helpers
{
   using Domain.Options;
   using Interceptors;

   internal static class InterceptorHelper
   {
      #region Methods

      internal static Option<TInterceptor> As<TInterceptor>(this IInterceptor interceptor)
         where TInterceptor : IInterceptor
      {
         return (TInterceptor)interceptor;
      }

      #endregion
   }
}