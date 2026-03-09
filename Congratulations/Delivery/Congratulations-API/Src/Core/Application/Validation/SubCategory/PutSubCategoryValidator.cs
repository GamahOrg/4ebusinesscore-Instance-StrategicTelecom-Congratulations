

namespace _4EPlatform.Application.Validation.SubCategory
{
    public class PutSubCategoryValidator : AbstractValidator<PutSubCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;
        public PutSubCategoryValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> stringLocalizer)
        {

            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.putSubCategoryDto.NameAr)
              .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameArabicMustNotEmpty] ?? "ERROR Localizer")
              .Matches(@"^[\u0600-\u06FF0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x.putSubCategoryDto.NameEn)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameEnglishMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[A-Za-z0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x.putSubCategoryDto.DescriptionAr)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.DescriptionArabicMustNotEmpty] ?? "ERROR Localizer")
               .Matches(@"^[\u0600-\u06FF0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.DescriptionInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x.putSubCategoryDto.DescriptionEn)
              .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.DescriptionEnglishMustNotEmpty] ?? "ERROR Localizer")
              .Matches(@"^[A-Za-z0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.DescriptionInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x)
                .Must(IsUniqueNameCombination)
                .WithMessage(_stringLocalizer[SharedResourceKeys.TheCombinationOfNameArAndNameEnMustBeUnique]);
            _stringLocalizer = stringLocalizer;
        }
        private bool IsUniqueNameCombination(PutSubCategoryCommand command)
        {
            var repository = _unitOfWork.Repository<SubCatgeory>();
            var existing = repository.GetAllAsTracking(x =>
            (x.NameAr == command.putSubCategoryDto.NameAr || x.NameEn == command.putSubCategoryDto.NameEn) &&
            x.State != State.Deleted
            && x.Id != command.Id &&
            x.CategoryId == command.putSubCategoryDto.CategoryId).FirstOrDefault();

            return existing == null;
        }
    }
}
