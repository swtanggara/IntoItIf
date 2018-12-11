namespace IntoItIf.Base.Domain.Entities
{
   using System.Threading.Tasks;
   using Mappers;
   using Services;
   using Validations;

   public abstract class BaseDto<TDto, TValidator> : BaseDto<TDto>
      where TDto : BaseDto<TDto, TValidator>
      where TValidator : IDataValidator<TDto>, new()
   {
      #region Constructors and Destructors

      protected BaseDto() : this(new TValidator())
      {
      }

      protected BaseDto(IDataValidator<TDto> validator) : base(validator)
      {
      }

      protected BaseDto(IMapperService mapperService, IDataValidator<TDto> validator) : base(
         mapperService,
         validator)
      {
      }

      #endregion
   }

   public abstract class BaseDto<TDto> : ValueObject<TDto>, IDto
      where TDto : BaseDto<TDto>
   {
      #region Constructors and Destructors

      protected BaseDto(IDataValidator<TDto> validator) : this(
         InjecterGetter.GetBaseMapperService(),
         validator)
      {
      }

      protected BaseDto(IMapperService mapperService, IDataValidator<TDto> validator)
      {
         MapperService = mapperService;
         DataValidator = validator;
      }

      #endregion

      #region Properties

      protected IDataValidator<TDto> DataValidator { get; }
      protected IMapperService MapperService { get; }
      protected TDto This => this as TDto;

      #endregion

      #region Public Methods and Operators

      public TEntity ToEntity<TEntity>()
         where TEntity : class, IEntity
      {
         return MapperService.ToEntity<TDto, TEntity>(This);
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