/*
 * https://github.com/HeadspringLabs/Enumeration
 */

namespace IntoItIf.Base.Domain
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Linq;
   using System.Reflection;
   using System.Runtime.Serialization;

   [Serializable]
   [DebuggerDisplay("{Name} - {Id}")]
   public abstract class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
      where TEnumeration : Enumeration<TEnumeration>
   {
      #region Constructors and Destructors

      protected Enumeration(int id, string name)
         : base(id, name)
      {
      }

      #endregion

      #region Public Methods and Operators

      public static TEnumeration FromInt32(int value)
      {
         return FromValue(value);
      }

      public static bool TryFromInt32(int listItemValue, out TEnumeration result)
      {
         return TryParse(listItemValue, out result);
      }

      #endregion
   }

   [Serializable]
   [DebuggerDisplay("{Name} - {Id}")]
   [DataContract(Namespace = "http://github.com/HeadspringLabs/Enumeration/5/13")]
   public abstract class Enumeration<TEnumeration, TValue>
      : ValueObject<TEnumeration>, IComparable<TEnumeration>, IEquatable<TEnumeration>, IEnumeration<TValue>
      where TEnumeration : Enumeration<TEnumeration, TValue>
      where TValue : IComparable
   {
      #region Static Fields

      private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

      #endregion

      #region Constructors and Destructors

      protected Enumeration(TValue id, string name)
      {
         if (id == null)
         {
            throw new ArgumentNullException(nameof(id));
         }

         Id = id;
         Name = name;
      }

      #endregion

      #region Public Properties

      [field: DataMember(Order = 0)]
      public TValue Id { get; }

      [field: DataMember(Order = 1)]
      public string Name { get; }

      #endregion

      #region Public Methods and Operators

      public static TEnumeration FromValue(TValue value)
      {
         return Parse(value, "value", item => item.Id.Equals(value));
      }

      public static TEnumeration[] GetAll()
      {
         return Enumerations.Value;
      }

      public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
      {
         return Equals(left, right);
      }

      public static implicit operator TValue(Enumeration<TEnumeration, TValue> source)
      {
         return source.Id;
      }

      public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
      {
         return !Equals(left, right);
      }

      public static TEnumeration Parse(string displayName)
      {
         return Parse(displayName, "display name", item => item.Name == displayName);
      }

      public static bool TryParse(TValue value, out TEnumeration result)
      {
         return TryParse(e => e.ValueEquals(value), out result);
      }

      public static bool TryParse(string displayName, out TEnumeration result)
      {
         return TryParse(e => e.Name == displayName, out result);
      }

      public int CompareTo(TEnumeration other)
      {
         return Id.CompareTo(other == default(TEnumeration) ? default : other.Id);
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as TEnumeration);
      }

      public bool Equals(TEnumeration other)
      {
         return other != null && ValueEquals(other.Id);
      }

      public override int GetHashCode()
      {
         return Id.GetHashCode();
      }

      public sealed override string ToString()
      {
         return Name;
      }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Id;
         yield return Name;
      }

      protected virtual bool ValueEquals(TValue value)
      {
         return Id.Equals(value);
      }

      private static TEnumeration[] GetEnumerations()
      {
         var enumerationType = typeof(TEnumeration);
         return enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
            .Select(info => info.GetValue(null))
            .Cast<TEnumeration>()
            .ToArray();
      }

      private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
      {
         if (TryParse(predicate, out var result)) return result;
         var message = $"'{value}' is not a valid {description} in {typeof(TEnumeration)}";
         throw new ArgumentException(message, nameof(value));
      }

      private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
      {
         result = GetAll().FirstOrDefault(predicate);
         return result != null;
      }

      #endregion
   }
}