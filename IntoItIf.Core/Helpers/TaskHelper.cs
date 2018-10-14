namespace IntoItIf.Core.Helpers
{
   using System.Threading.Tasks;

   public static class TaskHelper
   {
      public static Task<T> GetTask<T>(this T source)
      {
         return Task.FromResult(source);
      }
   }
}