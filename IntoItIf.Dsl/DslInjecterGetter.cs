namespace IntoItIf.Dsl
{
   using Core.Domain;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;

   public class DslInjecterGetter : BaseInjecterGetter
   {
      #region Static Fields

      private static Option<BaseUnitOfWork> _unitOfWork;

      #endregion

      #region Methods

      internal static Option<BaseUnitOfWork> GetBaseUnitOfWork()
      {
         return _unitOfWork;
      }

      internal static Option<bool> SetBaseUnitOfWork(Option<BaseUnitOfWork> uow)
      {
         return _unitOfWork
            .Combine(uow)
            .Execute(
               x =>
               {
                  var map = (_UnitOfWork: x.Item1, UnitOfWork: x.Item2);
                  if (map._UnitOfWork == null || map._UnitOfWork.GetType() != map.UnitOfWork.GetType())
                  {
                     map._UnitOfWork = map.UnitOfWork;
                  }
               });
      }

      #endregion
   }
}