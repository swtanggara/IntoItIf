namespace IntoItIf.Tests
{
   using Core.Domain.Entities;

   public class MyDto : BaseDto<MyDto, MyDtoFluentValidator>
   {
      public int Id { get; set; }
      public string Name { get; set; }
   }
}