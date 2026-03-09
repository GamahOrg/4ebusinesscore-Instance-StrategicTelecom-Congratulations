public class PostCategoryValidator : AbstractValidator<PostCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public readonly IStringLocalizer<SharedResource> _stringLocalizer;

    public PostCategoryValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> stringLocalizer)
    {
        _unitOfWork = unitOfWork;
        _stringLocalizer = stringLocalizer;

        RuleFor(x => x.postCategoryDto.NameAr)
            .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameArabicMustNotEmpty] ?? "ERROR Localizer")
            .Matches(@"^[\u0600-\u06FF0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");

        RuleFor(x => x.postCategoryDto.NameEn)
            .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameEnglishMustNotEmpty] ?? "ERROR Localizer")
            .Matches(@"^[A-Za-z0-9\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer");

        RuleFor(x => x)
            .Must(IsUniqueNameCombination)
            .WithMessage(_stringLocalizer[SharedResourceKeys.TheCombinationOfNameArAndNameEnMustBeUnique] ?? "ERROR Localizer");
    }

    private bool IsUniqueNameCombination(PostCategoryCommand command)
    {
        var repository = _unitOfWork.Repository<Category>();
        var existing = repository.GetAllAsTracking(x =>
            (x.NameAr == command.postCategoryDto.NameAr || x.NameEn == command.postCategoryDto.NameEn) &&
            x.State != State.Deleted).FirstOrDefault();

        return existing == null;
    }
}
