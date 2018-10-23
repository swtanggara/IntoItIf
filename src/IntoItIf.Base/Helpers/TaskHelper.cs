namespace IntoItIf.Base.Helpers
{
   using System.Threading.Tasks;

   public static class TaskHelper
   {
      #region Public Methods and Operators

      public static Task<T> GetTask<T>(this T source)
      {
         return Task.FromResult(source);
      }

      #endregion
   }
}