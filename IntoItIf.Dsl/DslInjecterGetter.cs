namespace IntoItIf.Dsl
{
   using Core.Domain;
   using Core.Domain.Options;
   using Dal.UnitOfWorks;

   public class DslInjecterGetter : InjecterGetter
   {
      #region Static Fields

      private static Option<BaseUnitOfWork> _unitOfWork;

      #endregion

      #region Methods

      internal static Option<BaseUnitOfWork> GetBaseUnitOfWork()
      {
         return _unitOfWork;
      }

      public static Option<bool> SetBaseUnitOfWork(Option<BaseUnitOfWork> uow)
      {
         var realClassUnitOfWork = _unitOfWork.ReduceOrDefault();
         var realUnitOfWork = uow.ReduceOrDefault();
         if (realClassUnitOfWork == null || realClassUnitOfWork.GetType() != realUnitOfWork.GetType())
         {
            _unitOfWork = uow;
         }

         return true;
      }

      #endregion
   }
}