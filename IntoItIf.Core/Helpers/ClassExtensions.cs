namespace IntoItIf.Core.Helpers
{
   using System.Linq;

   internal static class ClassExtensions
   {
      #region Methods

      internal static object GetPropertyValue<T>(this T instance, string propertyName)
      {
         var typeOfT = typeof(T);
         var property = typeOfT.GetProperty(propertyName);
         var result = property?.GetValue(instance);
         return result;
      }

      internal static string[] GetValidatedPropertyNames<T>(this string[] propertyNames)
         where T : class
      {
         var tType = typeof(T);
         var result = propertyNames.Select(x => new { PropertyName = x, PropertyInfo = tType.GetProperty(x) })
            .Where(x => x.PropertyInfo != null && x.PropertyInfo.CanWrite)
            .Select(x => x.PropertyName)
            .ToArray();
         return result;
      }

      #endregion
   }
}