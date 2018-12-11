namespace IntoItIf.Base.AutoMapper
{
   using Domain.Entities;
   using global::AutoMapper;
   using global::AutoMapper.Configuration;
   using Mappers;

   public sealed class AutoMapperService : IMapperService
   {
      public bool Initialize(params IMapperProfile[] mapperProfiles)
      {
         var config = new MapperConfigurationExpression();
         foreach (var mapperProfile in mapperProfiles)
         {
            foreach (var bind in mapperProfile.GetBinds())
            {
               config.CreateMap(bind.Source, bind.Destination);
            }
         }

         Mapper.Initialize(config);
         return true;
      }

      public TDto ToDto<T, TDto>(T entity)
         where T : class, IEntity where TDto : class, IDto
      {
         return Mapper.Map<T, TDto>(entity);
      }

      public T ToEntity<TDto, T>(TDto dto)
         where TDto : class, IDto where T : class, IEntity
      {
         return Mapper.Map<TDto, T>(dto);
      }
   }
}