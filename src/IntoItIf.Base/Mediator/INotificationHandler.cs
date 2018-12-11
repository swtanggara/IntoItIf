namespace IntoItIf.Base.Mediator
{
   using System.Threading;
   using System.Threading.Tasks;

   public interface INotificationHandler<in TNotification>
      where TNotification : INotification
   {
      #region Public Methods and Operators

      Task<bool> Handle(TNotification notification, CancellationToken ctok);

      #endregion
   }
}