namespace IntoItIf.Tests.Reflections
{
   using System;
   using Shouldly;
   using Xunit;

   public class StacktraceTest
   {
      [Fact]
      public void ThisMethodShouldShow()
      {
         var seed = IntSeedAndCaller;
         var increment = seed.Seed + 1;
         var stackTrace = Sum(increment);
         seed.Caller.ShouldBe("IntoItIf.Tests.Reflections.StacktraceTest");
      }

      private string Sum(int increment)
      {
         return StacktraceHelper.GetRootCallerMethodName();
      }

      public (int Seed, string Caller) IntSeedAndCaller => new StacktraceRouting().IntSeedAndCaller;
   }
}