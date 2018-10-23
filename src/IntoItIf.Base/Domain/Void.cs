namespace IntoItIf.Base.Domain
{
   using System;

   public sealed class Void : IEquatable<Void>, IComparable<Void>, IComparable
   {
      #region Constructors and Destructors

      private Void()
      {
      }

      #endregion

      #region Public Properties

      public static Void Value { get; } = new Void();

      #endregion

      #region Public Methods and Operators

      public static bool operator ==(Void first, Void second)
      {
         return true;
      }

      public static bool operator !=(Void first, Void second)
      {
         return false;
      }

      public int CompareTo(Void other)
      {
         return 0;
      }

      public bool Equals(Void other)
      {
         return true;
      }

      public override bool Equals(object obj)
      {
         return obj is Void;
      }

      public override int GetHashCode()
      {
         return 0;
      }

      public override string ToString()
      {
         return "()";
      }

      #endregion

      #region Explicit Interface Methods

      int IComparable.CompareTo(object obj)
      {
         return 0;
      }

      #endregion
   }
}