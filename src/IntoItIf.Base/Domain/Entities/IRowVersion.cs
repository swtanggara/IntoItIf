namespace IntoItIf.Base.Domain.Entities
{
   public interface IRowVersion
   {
      #region Public Properties

      byte[] RowVersion { get; }

      #endregion
   }
}