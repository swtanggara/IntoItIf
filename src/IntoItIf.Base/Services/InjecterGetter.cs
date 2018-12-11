namespace IntoItIf.Base.Services
{
   using Mappers;
   using UnitOfWork;

   public sealed class InjecterGetter
   {
      #region Static Fields

      private static IMapperService _mapperService;
      private static ISaveUow _unitOfWork;

      #endregion

      #region Public Methods and Operators

      public static bool SetBaseMapperService(IMapperService mapperService)
      {
         if (_mapperService == null || _mapperService.GetType() != mapperService.GetType())
         {
            _mapperService = mapperService;
         }

         return true;
      }

      public static bool SetUow(ISaveUow uow)
      {
         if (_unitOfWork == null || _unitOfWork.GetType() != uow.GetType())
         {
            _unitOfWork = uow;
         }

         return true;
      }

      #endregion

      #region Methods

      internal static IMapperService GetBaseMapperService()
      {
         return _mapperService;
      }

      internal static ISaveUow GetSaveUow()
      {
         return _unitOfWork;
      }

      #endregion
   }
}