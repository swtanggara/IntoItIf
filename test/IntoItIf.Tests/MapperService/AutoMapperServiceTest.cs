namespace IntoItIf.Tests.MapperService
{
   using Preparation;
   using Preparation.Preparation;
   using Shouldly;
   using Xunit;
   using Xunit.Abstractions;

   public class AutoMapperServiceTest
   {
      #region Fields

      private readonly ITestOutputHelper _output;

      #endregion

      #region Constructors and Destructors

      public AutoMapperServiceTest(ITestOutputHelper output)
      {
         _output = output;
         MapperTestHelper.InitAutoMapper();
      }

      #endregion

      #region Public Methods and Operators

      [Fact]
      public void DtoSeed_ToEntity_ShouldNotNull_And_ShouldBeSameAs_ExpectedEntity()
      {
         var entity = MapperTestHelper.DtoSeed.ToEntity<MyEntity>();
         _output.WriteLine(
            $"DtoSeed: {MapperTestHelper.DtoSeed} => entity: {entity} => ExpectedEntity: {MapperTestHelper.ExpectedEntity}");
         entity.ShouldNotBeNull();
         entity.ShouldBe(MapperTestHelper.ExpectedEntity);
      }

      [Fact]
      public void DtoSeed_ToEntity_ShouldNotNull_And_ShouldNotSameAs_UnexpectedEntity()
      {
         var entity = MapperTestHelper.DtoSeed.ToEntity<MyEntity>();
         _output.WriteLine(
            $"DtoSeed: {MapperTestHelper.DtoSeed} => entity: {entity} => UnexpectedEntity: {MapperTestHelper.UnexpectedEntity}");
         entity.ShouldNotBeNull();
         entity.ShouldNotBe(MapperTestHelper.UnexpectedEntity);
      }

      [Fact]
      public void EntitySeed_ToDto_ShouldNotNull_And_ShouldBeSameAs_ExpectedDto()
      {
         var dto = MapperTestHelper.EntitySeed.ToDto<MyDto>();
         _output.WriteLine(
            $"EntitySeed: {MapperTestHelper.EntitySeed} => dto: {dto} => ExpectedDto: {MapperTestHelper.ExpectedDto}");
         dto.ShouldNotBeNull();
         dto.ShouldBe(MapperTestHelper.ExpectedDto);
      }

      [Fact]
      public void EntitySeed_ToDt_ShouldNotNull_And_ShouldNotSameAs_UnexpectedDto()
      {
         var dto = MapperTestHelper.EntitySeed.ToDto<MyDto>();
         _output.WriteLine(
            $"EntitySeed: {MapperTestHelper.EntitySeed} => dto: {dto} => UnexpectedDto: {MapperTestHelper.UnexpectedDto}");
         dto.ShouldNotBeNull();
         dto.ShouldNotBe(MapperTestHelper.UnexpectedDto);
      }

      #endregion
   }
}