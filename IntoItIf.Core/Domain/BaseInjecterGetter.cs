namespace IntoItIf.Core.Domain
{
   using Options;

   public abstract class BaseInjecterGetter
   {
      #region Static Fields

      private static Option<IMapperService> _mapperService;

      #endregion

      #region Methods

      internal static Option<IMapperService> GetBaseMapperService()
      {
         return _mapperService;
      }

      internal static Option<bool> SetBaseMapperService(Option<IMapperService> mapperService)
      {
         return _mapperService
            .Combine(mapperService)
            .Execute(
               x =>
               {
                  var map = (_MapperService: x.Item1, MapperService: x.Item2);
                  if (map._MapperService == null || map._MapperService.GetType() != map.MapperService.GetType())
                  {
                     map._MapperService = map.MapperService;
                  }
               });
      }

      #endregion
   }
}