namespace IntoItIf.Tests.Reflections
{
   using System;
   using System.Diagnostics;
   using System.Runtime.CompilerServices;

   public class StacktraceHelper
   {
      public static string GetRootCallerMethodName()
      {
         string fullName;
         Type declaringType;
         var skipFrames = 2;
         do
         {
            var method = new StackFrame(skipFrames, false).GetMethod();
            declaringType = method.DeclaringType;
            if (declaringType == null)
            {
               return method.Name;
            }
            skipFrames++;
            fullName = $"{declaringType.FullName}.{method.Name}";
         }
         while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

         return fullName;
      }
   }
}