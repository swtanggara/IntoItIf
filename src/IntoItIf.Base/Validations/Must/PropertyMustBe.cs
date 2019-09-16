namespace IntoItIf.Base.Validations.Must
{
   using System.Collections.Generic;
   using Domain;

   public class PropertyMustBe<T, TProp> : ValueObject<PropertyMustBe<T, TProp>>

   {
      #region Constructors and Destructors

      internal PropertyMustBe(T parent, string name, TProp value)
      {
         Parent = parent;
         Name = name;
         Value = value;
      }

      #endregion

      #region Public Properties

      public T Parent { get; }
      public string Name { get; }
      public TProp Value { get; }

      #endregion

      #region Methods

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return Parent;
         yield return Name;
         yield return Value;
      }

      #endregion
   }
}