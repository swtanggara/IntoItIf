namespace IntoItIf.Base.Services
{
   using Domain.Options;
   using Mappers;
   using UnitOfWork;

   public sealed class InjecterGetter
   {
      #region Static Fields

      private static Option<IMapperService> _mapperService;
      private static Option<ISaveUow> _unitOfWork;

      #endregion

      #region Public Methods and Operators

      public static Option<bool> SetBaseMapperService(Option<IMapperService> mapperService)
      {
         var realClassMapperService = _mapperService.ReduceOrDefault();
         var realMapperService = mapperService.ReduceOrDefault();
         if (realClassMapperService == null || realClassMapperService.GetType() != realMapperService.GetType())
         {
            _mapperService = mapperService;
         }

         return true;
      }

      public static Option<bool> SetUow(Option<ISaveUow> uow)
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

      #region Methods

      internal static Option<IMapperService> GetBaseMapperService()
      {
         return _mapperService;
      }

      internal static Option<ISaveUow> GetSaveUow()
      {
         return _unitOfWork;
      }

      #endregion
   }
}