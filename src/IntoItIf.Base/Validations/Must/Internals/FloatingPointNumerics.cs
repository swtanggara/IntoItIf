namespace IntoItIf.Base.Validations.Must.Internals
{
   using System;
   using System.Runtime.InteropServices;

   internal static class FloatingPointNumerics
   {
      public static bool AreAlmostEqualUlps(float left, float right, int maxUlps)
      {
         FloatIntUnion floatIntUnion1 = new FloatIntUnion();
         FloatIntUnion floatIntUnion2 = new FloatIntUnion();
         floatIntUnion1.Float = left;
         floatIntUnion2.Float = right;
         uint num1 = floatIntUnion1.UInt >> 31;
         uint num2 = floatIntUnion2.UInt >> 31;
         uint num3 = 2147483648U - floatIntUnion1.UInt & num1;
         floatIntUnion1.UInt = num3 | floatIntUnion1.UInt & ~num1;
         uint num4 = 2147483648U - floatIntUnion2.UInt & num2;
         floatIntUnion2.UInt = num4 | floatIntUnion2.UInt & ~num2;
         return Math.Abs(floatIntUnion1.Int - floatIntUnion2.Int) <= maxUlps;
      }

      public static bool AreAlmostEqualUlps(double left, double right, long maxUlps)
      {
         DoubleLongUnion doubleLongUnion1 = new DoubleLongUnion();
         DoubleLongUnion doubleLongUnion2 = new DoubleLongUnion();
         doubleLongUnion1.Double = left;
         doubleLongUnion2.Double = right;
         ulong num1 = doubleLongUnion1.ULong >> 63;
         ulong num2 = doubleLongUnion2.ULong >> 63;
         ulong num3 = 9223372036854775808UL - doubleLongUnion1.ULong & num1;
         doubleLongUnion1.ULong = num3 | doubleLongUnion1.ULong & ~num1;
         ulong num4 = 9223372036854775808UL - doubleLongUnion2.ULong & num2;
         doubleLongUnion2.ULong = num4 | doubleLongUnion2.ULong & ~num2;
         return Math.Abs(doubleLongUnion1.Long - doubleLongUnion2.Long) <= maxUlps;
      }

      public static int ReinterpretAsInt(float value)
      {
         return new FloatIntUnion
         {
            Float = value
         }.Int;
      }

      public static long ReinterpretAsLong(double value)
      {
         return new DoubleLongUnion
         {
            Double = value
         }.Long;
      }

      public static float ReinterpretAsFloat(int value)
      {
         return new FloatIntUnion
         {
            Int = value
         }.Float;
      }

      public static double ReinterpretAsDouble(long value)
      {
         return new DoubleLongUnion
         {
            Long = value
         }.Double;
      }

      [StructLayout(LayoutKind.Explicit)]
      private struct DoubleLongUnion
      {
         [FieldOffset(0)]
         public double Double;
         [FieldOffset(0)]
         public long Long;
         [FieldOffset(0)]
         public ulong ULong;
      }

      [StructLayout(LayoutKind.Explicit)]
      private struct FloatIntUnion
      {
         [FieldOffset(0)]
         public float Float;
         [FieldOffset(0)]
         public int Int;
         [FieldOffset(0)]
         public uint UInt;
      }
   }
}