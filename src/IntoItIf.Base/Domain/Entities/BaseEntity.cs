namespace IntoItIf.Base.Domain.Entities
{
   using System.Threading.Tasks;
   using Mappers;
   using Services;
   using Validations;

   public abstract class BaseEntity<TEntity, TValidator> : BaseEntity<TEntity>
      where TEntity : BaseEntity<TEntity>
      where TValidator : IDataValidator<TEntity>, new()
   {
      #region Constructors and Destructors

      protected BaseEntity() : this(new TValidator())
      {
      }

      protected BaseEntity(IDataValidator<TEntity> validator) : base(validator)
      {
      }

      protected BaseEntity(IMapperService mapperService, IDataValidator<TEntity> validator) : base(
         mapperService,
         validator)
      {
      }

      #endregion
   }

   public abstract class BaseEntity<TEntity> : ValueObject<TEntity>, IEntity
      where TEntity : BaseEntity<TEntity>
   {
      #region Constructors and Destructors

      protected BaseEntity() : this(new EmptyDataValidator<TEntity>())
      {
      }

      protected BaseEntity(IDataValidator<TEntity> validator) : this(
         InjecterGetter.GetBaseMapperService(),
         validator)
      {
      }

      protected BaseEntity(IMapperService mapperService, IDataValidator<TEntity> validator)
      {
         MapperService = mapperService;
         DataValidator = validator;
      }

      #endregion

      #region Properties

      protected IDataValidator<TEntity> DataValidator { get; }
      protected IMapperService MapperService { get; }
      protected TEntity This => this as TEntity;

      #endregion

      #region Public Methods and Operators

      public TDto ToDto<TDto>()
         where TDto : class, IDto
      {
         return MapperService.ToDto<TEntity, TDto>(This);
      }

      public ValidationResult Validate(params string[] args)
      {
         return DataValidator.Validate(This);
      }

      public Task<ValidationResult> ValidateAsync(params string[] args)
      {
         return DataValidator.ValidateAsync(This);
      }

      #endregion
   }
}