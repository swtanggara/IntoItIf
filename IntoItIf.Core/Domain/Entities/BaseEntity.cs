namespace IntoItIf.Core.Domain.Entities
{
   using System.Threading.Tasks;
   using Options;

   public abstract class BaseEntity<TEntity, TValidator> : BaseEntity<TEntity>
      where TEntity : BaseEntity<TEntity>
      where TValidator : IDataValidator<TEntity>, new()
   {
      #region Constructors and Destructors

      protected BaseEntity() : this(new TValidator())
      {
      }

      protected BaseEntity(Option<IDataValidator<TEntity>> validator) : base(validator)
      {
      }

      protected BaseEntity(Option<IMapperService> mapperService, Option<IDataValidator<TEntity>> validator) : base(
         mapperService,
         validator)
      {
      }

      #endregion
   }

   public abstract class BaseEntity<TEntity> : IEntity
      where TEntity : BaseEntity<TEntity>
   {
      #region Constructors and Destructors

      protected BaseEntity(Option<IDataValidator<TEntity>> validator) : this(
         BaseInjecterGetter.GetBaseMapperService(),
         validator)
      {
      }

      protected BaseEntity(Option<IMapperService> mapperService, Option<IDataValidator<TEntity>> validator)
      {
         MapperService = mapperService;
         DataValidator = validator;
      }

      #endregion

      #region Properties

      protected Option<IDataValidator<TEntity>> DataValidator { get; }
      protected Option<IMapperService> MapperService { get; }
      protected Option<TEntity> This => this as TEntity;

      #endregion

      #region Public Methods and Operators

      public Option<TDto> ToDto<TDto>()
         where TDto : class, IDto
      {
         return GetMapperServiceThisOption(This).MapFlatten(x => x.MapperService.ToDto<TEntity, TDto>(x.This));
      }

      public Option<ValidationResult> Validate(params string[] args)
      {
         return GetDataValidatorThisOption(This).MapFlatten(x => x.DataValidator.Validate(x.This));
      }

      public Task<Option<ValidationResult>> ValidateAsync(params string[] args)
      {
         return GetDataValidatorThisOption(This).MapFlattenAsync(x => x.DataValidator.ValidateAsync(x.This));
      }

      #endregion

      #region Methods

      protected Option<(IDataValidator<TEntity> DataValidator, TEntity This)> GetDataValidatorThisOption(
         Option<TEntity> dto)
      {
         return dto.Combine(DataValidator).Map(x => (DataValidator: x.Item2, This: x.Item1));
      }

      protected Option<(IMapperService MapperService, TEntity This)> GetMapperServiceThisOption(Option<TEntity> dto)
      {
         return dto.Combine(MapperService).Map(x => (MapperService: x.Item2, This: x.Item1));
      }

      #endregion
   }
}