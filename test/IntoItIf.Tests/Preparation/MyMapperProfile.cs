namespace IntoItIf.Tests.Preparation
{
   using System;
   using Base.Mappers;

   public class MyMapperProfile : IMapperProfile
   {
      public (Type Source, Type Destination)[] GetBinds()
      {
         return new (Type Source, Type Destination)[]
         {
             (typeof(MyEntity), typeof(MyDto)),
             (typeof(MyDto), typeof(MyEntity))
         };
      }
   }
}
