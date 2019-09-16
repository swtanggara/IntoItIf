namespace IntoItIf.Base.Validations.Must.Internals
{
   using System;

   internal static class Numerics
   {
      public static bool IsNumericType(object obj)
      {
         return IsFloatingPointNumeric(obj) || IsFixedPointNumeric(obj);
      }

      public static bool IsFloatingPointNumeric(object obj)
      {
         return obj != null && (obj is double || obj is float);
      }

      public static bool IsFixedPointNumeric(object obj)
      {
         return obj != null && (obj is byte || obj is sbyte || (obj is decimal || obj is int) || (obj is uint || obj is long || (obj is ulong || obj is short)) || obj is ushort);
      }

      public static bool AreEqual(object expected, object actual, ref Tolerance tolerance)
      {
         if (expected is double || actual is double)
            return AreEqual(Convert.ToDouble(expected), Convert.ToDouble(actual), ref tolerance);
         if (expected is float || actual is float)
            return AreEqual(Convert.ToSingle(expected), Convert.ToSingle(actual), ref tolerance);
         if (tolerance.Mode == ToleranceMode.Ulps)
            throw new InvalidOperationException("Ulps may only be specified for floating point arguments");
         if (expected is decimal || actual is decimal)
            return AreEqual(Convert.ToDecimal(expected), Convert.ToDecimal(actual), tolerance);
         if (expected is ulong || actual is ulong)
            return AreEqual(Convert.ToUInt64(expected), Convert.ToUInt64(actual), tolerance);
         if (expected is long || actual is long)
            return AreEqual(Convert.ToInt64(expected), Convert.ToInt64(actual), tolerance);
         if (expected is uint || actual is uint)
            return AreEqual(Convert.ToUInt32(expected), Convert.ToUInt32(actual), tolerance);
         return AreEqual(Convert.ToInt32(expected), Convert.ToInt32(actual), tolerance);
      }

      private static bool AreEqual(double expected, double actual, ref Tolerance tolerance)
      {
         if (double.IsNaN(expected) && double.IsNaN(actual))
            return true;
         if (double.IsInfinity(expected) || double.IsNaN(expected) || double.IsNaN(actual))
            return expected.Equals(actual);
         if (tolerance.IsEmpty)
            tolerance = new Tolerance(double.Epsilon);
         switch (tolerance.Mode)
         {
            case ToleranceMode.None:
               return expected.Equals(actual);
            case ToleranceMode.Linear:
               return Math.Abs(expected - actual) <= Convert.ToDouble(tolerance.Value);
            case ToleranceMode.Percent:
               if (expected == 0.0)
                  return expected.Equals(actual);
               return Math.Abs((expected - actual) / expected) <= Convert.ToDouble(tolerance.Value) / 100.0;
            case ToleranceMode.Ulps:
               return FloatingPointNumerics.AreAlmostEqualUlps(expected, actual, Convert.ToInt64(tolerance.Value));
            default:
               throw new ArgumentException("Unknown tolerance mode specified", nameof(tolerance.Mode));
         }
      }

      private static bool AreEqual(float expected, float actual, ref Tolerance tolerance)
      {
         if (float.IsNaN(expected) && float.IsNaN(actual))
            return true;
         if (float.IsInfinity(expected) || float.IsNaN(expected) || float.IsNaN(actual))
            return expected.Equals(actual);
         if (tolerance.IsEmpty)
            tolerance = new Tolerance(double.Epsilon);
         switch (tolerance.Mode)
         {
            case ToleranceMode.None:
               return expected.Equals(actual);
            case ToleranceMode.Linear:
               return Math.Abs(expected - actual) <= Convert.ToDouble(tolerance.Value);
            case ToleranceMode.Percent:
               if (expected == 0.0)
                  return expected.Equals(actual);
               return Math.Abs((expected - actual) / expected) <= Convert.ToSingle(tolerance.Value) / 100.0;
            case ToleranceMode.Ulps:
               return FloatingPointNumerics.AreAlmostEqualUlps(expected, actual, Convert.ToInt32(tolerance.Value));
            default:
               throw new ArgumentException("Unknown tolerance mode specified", "mode");
         }
      }

      private static bool AreEqual(decimal expected, decimal actual, Tolerance tolerance)
      {
         switch (tolerance.Mode)
         {
            case ToleranceMode.None:
               return expected.Equals(actual);
            case ToleranceMode.Linear:
               decimal num = Convert.ToDecimal(tolerance.Value);
               if (num > decimal.Zero)
                  return Math.Abs(expected - actual) <= num;
               return expected.Equals(actual);
            case ToleranceMode.Percent:
               if (expected == decimal.Zero)
                  return expected.Equals(actual);
               return Math.Abs((double)(expected - actual) / (double)expected) <= Convert.ToDouble(tolerance.Value) / 100.0;
            default:
               throw new ArgumentException("Unknown tolerance mode specified", "mode");
         }
      }

      private static bool AreEqual(ulong expected, ulong actual, Tolerance tolerance)
      {
         switch (tolerance.Mode)
         {
            case ToleranceMode.None:
               return expected.Equals(actual);
            case ToleranceMode.Linear:
               ulong uint64 = Convert.ToUInt64(tolerance.Value);
               if (uint64 > 0UL)
                  return (expected >= actual ? expected - actual : actual - expected) <= uint64;
               return expected.Equals(actual);
            case ToleranceMode.Percent:
               if (expected == 0UL)
                  return expected.Equals(actual);
               return Math.Abs((Math.Max(expected, actual) - Math.Min(expected, actual)) / (double)expected) <= Convert.ToDouble(tolerance.Value) / 100.0;
            default:
               throw new ArgumentException("Unknown tolerance mode specified", "mode");
         }
      }

      private static bool AreEqual(long expected, long actual, Tolerance tolerance)
      {
         switch (tolerance.Mode)
         {
            case ToleranceMode.None:
               return expected.Equals(actual);
            case ToleranceMode.Linear:
               long int64 = Convert.ToInt64(tolerance.Value);
               if (int64 > 0L)
                  return Math.Abs(expected - actual) <= int64;
               return expected.Equals(actual);
            case ToleranceMode.Percent:
               if (expected == 0L)
                  return expected.Equals(actual);
               return Math.Abs((expected - actual) / (double)expected) <= Convert.ToDouble(tolerance.Value) / 100.0;
            default:
               throw new ArgumentException("Unknown tolerance mode specified", "mode");
         }
      }

      private static bool AreEqual(uint expected, uint actual, Tolerance tolerance)
      {
         switch (tolerance.Mode)
         {
            case ToleranceMode.None:
               return expected.Equals(actual);
            case ToleranceMode.Linear:
               uint uint32 = Convert.ToUInt32(tolerance.Value);
               if (uint32 > 0U)
                  return (expected >= actual ? expected - actual : actual - expected) <= uint32;
               return expected.Equals(actual);
            case ToleranceMode.Percent:
               if (expected == 0U)
                  return expected.Equals(actual);
               return Math.Abs((Math.Max(expected, actual) - Math.Min(expected, actual)) / (double)expected) <= Convert.ToDouble(tolerance.Value) / 100.0;
            default:
               throw new ArgumentException("Unknown tolerance mode specified", "mode");
         }
      }

      private static bool AreEqual(int expected, int actual, Tolerance tolerance)
      {
         switch (tolerance.Mode)
         {
            case ToleranceMode.None:
               return expected.Equals(actual);
            case ToleranceMode.Linear:
               int int32 = Convert.ToInt32(tolerance.Value);
               if (int32 > 0)
                  return Math.Abs(expected - actual) <= int32;
               return expected.Equals(actual);
            case ToleranceMode.Percent:
               if (expected == 0)
                  return expected.Equals(actual);
               return Math.Abs((expected - actual) / (double)expected) <= Convert.ToDouble(tolerance.Value) / 100.0;
            default:
               throw new ArgumentException("Unknown tolerance mode specified", "mode");
         }
      }

      public static int Compare(object expected, object actual)
      {
         if (!IsNumericType(expected) || !IsNumericType(actual))
            throw new ArgumentException("Both arguments must be numeric");
         if (IsFloatingPointNumeric(expected) || IsFloatingPointNumeric(actual))
            return Convert.ToDouble(expected).CompareTo(Convert.ToDouble(actual));
         if (expected is decimal || actual is decimal)
            return Convert.ToDecimal(expected).CompareTo(Convert.ToDecimal(actual));
         if (expected is ulong || actual is ulong)
            return Convert.ToUInt64(expected).CompareTo(Convert.ToUInt64(actual));
         if (expected is long || actual is long)
            return Convert.ToInt64(expected).CompareTo(Convert.ToInt64(actual));
         if (expected is uint || actual is uint)
            return Convert.ToUInt32(expected).CompareTo(Convert.ToUInt32(actual));
         return Convert.ToInt32(expected).CompareTo(Convert.ToInt32(actual));
      }
   }
}