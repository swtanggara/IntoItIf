namespace IntoItIf.Core.Mediator
{
   using System.Threading;
   using System.Threading.Tasks;
   using Domain.Options;

   public interface INotificationHandler<TNotification>
      where TNotification : INotification
   {
      #region Public Methods and Operators

      Task<Option<bool>> Handle(Option<TNotification> notification, Option<CancellationToken> ctok);

      #endregion
   }
}