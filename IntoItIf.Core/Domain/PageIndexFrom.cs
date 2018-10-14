namespace IntoItIf.Core.Domain
{
   public class PageIndexFrom : Enumeration<PageIndexFrom>
   {
      #region Static Fields

      public static readonly PageIndexFrom One = new PageIndexFrom(1, nameof(One));

      public static readonly PageIndexFrom Zero = new PageIndexFrom(0, nameof(Zero));

      #endregion

      #region Constructors and Destructors

      private PageIndexFrom(int id, string name) : base(id, name)
      {
      }

      #endregion
   }
}