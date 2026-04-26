
namespace _4EPlatform.Application.Validation.CategoryValidation
{
    public class PutCategoryValidator : AbstractValidator<PutCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PutCategoryValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;

            RuleFor(x => x.putCategoryDto.NameAr)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameArabicMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[\u0600-\u06FF0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");



            RuleFor(x => x.putCategoryDto.NameEn)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameEnglishMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[A-Za-z0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer");



            RuleFor(x => x)
                .Must(IsUniqueNameCombination)
                .WithMessage(_stringLocalizer[SharedResourceKeys.TheCombinationOfNameArAndNameEnMustBeUnique] ?? "ERROR Localizer");
            _stringLocalizer = stringLocalizer;
        }
        private bool IsUniqueNameCombination(PutCategoryCommand command)
        {
            var repository = _unitOfWork.Repository<Category>();
            var existing = repository.GetAllAsTracking(x =>
            (x.NameAr == command.putCategoryDto.NameAr || x.NameEn == command.putCategoryDto.NameEn) &&
            x.State != State.Deleted
            && x.Id != command.putCategoryDto.Id).FirstOrDefault();

            return existing == null;
        }
    }
}
