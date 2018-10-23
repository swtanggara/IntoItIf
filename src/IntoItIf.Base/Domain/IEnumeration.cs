namespace IntoItIf.Base.Domain
{
   using System;
   using Services;

   public interface IEnumeration<out TValue> : IInjectable
      where TValue : IComparable
   {
      #region Public Properties

      TValue Id { get; }
      string Name { get; }

      #endregion
   }
}