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

      public static T ToSync<T>(this Task<T> task)
      {
         return AsyncHelper.RunSync(() => task);
      }

      public static void RunSync(this Task task)
      {
         AsyncHelper.RunSync(() => task);
      }

      #endregion
   }
}