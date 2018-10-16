namespace IntoItIf.Tests.MapperService
{
   using Core.Domain;
   using Dsl.Automapper;
   using Dsl.BatMap;
   using Dsl.Mapster;
   using Preparation;

   internal class MapperTestHelper
   {
      internal static readonly MyDto DtoSeed = new MyDto(2, "Baraka");
      internal static readonly MyEntity EntitySeed = new MyEntity(1, "Angga");
      internal static readonly MyDto ExpectedDto = new MyDto(1, "Angga");
      internal static readonly MyEntity ExpectedEntity = new MyEntity(2, "Baraka");
      internal static readonly MyDto UnexpectedDto = new MyDto(1, "angga");
      internal static readonly MyEntity UnexpectedEntity = new MyEntity(2, "baraka");

      internal static void InitAutoMapper()
      {
         var mapperSvc = new AutoMapperService();
         var profile = new MyMapperProfile();
         mapperSvc.Initialize<MyMapperProfile>(profile);
         InjecterGetter.SetBaseMapperService(mapperSvc);
      }

      internal static void InitMapster()
      {
         var mapperSvc = new MapsterMapperService();
         var profile = new MyMapperProfile();
         mapperSvc.Initialize<MyMapperProfile>(profile);
         InjecterGetter.SetBaseMapperService(mapperSvc);
      }

      internal static void InitBatMap()
      {
         var mapperSvc = new BatMapMapperService();
         var profile = new MyMapperProfile();
         mapperSvc.Initialize<MyMapperProfile>(profile);
         InjecterGetter.SetBaseMapperService(mapperSvc);
      }

   }
}