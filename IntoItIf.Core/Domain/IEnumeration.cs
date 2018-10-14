namespace IntoItIf.Core.Domain
{
   using System;

   public interface IEnumeration<out TValue> : IInjectable
      where TValue : IComparable
   {
      #region Public Properties

      TValue Id { get; }
      string Name { get; }

      #endregion
   }
}