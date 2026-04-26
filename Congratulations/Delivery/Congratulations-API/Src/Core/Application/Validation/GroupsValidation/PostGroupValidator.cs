namespace _4EPlatform.Application.Validation.GroupsValidation
{
    public class PostGroupValidator : AbstractValidator<PostGroupCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;
        public PostGroupValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.postGroupDto.NameAr)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameArabicMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[\u0600-\u06FF\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x.postGroupDto.NameEn)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameEnglishMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[A-Za-z\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x.postGroupDto.EmployeesGroupsIds)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.PleaseSelectEmployees] ?? "ERROR Localizer");
            RuleFor(x => x)
             .Must(IsUniqueNameCombination)
             .WithMessage(_stringLocalizer[SharedResourceKeys.TheCombinationOfNameArAndNameEnMustBeUnique] ?? "ERROR Localizer");
            _stringLocalizer = stringLocalizer;
        }

        private bool IsUniqueNameCombination(PostGroupCommand command)
        {
            var repository = _unitOfWork.Repository<Groups>();
            var existing = repository.GetAllAsTracking(x =>
            (x.NameAr == command.postGroupDto.NameAr || x.NameEn == command.postGroupDto.NameEn) &&
            x.State != State.Deleted).FirstOrDefault();

            return existing == null;
        }
    }
}
