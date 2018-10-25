using System;

namespace IntoItIf.Tests.Reflections
{
   public class StacktraceRouting
    {
       internal (int Seed, string Caller) IntSeedAndCaller
       {
          get
          {
             var random = new Random(10);
             return (random.Next(20, 30), StacktraceHelper.GetRootCallerMethodName());
          }
      }

   }
}
