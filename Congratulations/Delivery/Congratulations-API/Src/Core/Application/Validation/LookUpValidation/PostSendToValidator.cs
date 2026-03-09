
namespace _4EPlatform.Application.Validation.LookUpValidation
{
    public class PostSendToValidator : AbstractValidator<PostSendToCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;
        public PostSendToValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> stringLocalizer)
        {

            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.postSendToDto.NameAr)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameArabicMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[\u0600-\u06FF\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");



            RuleFor(x => x.postSendToDto.NameEn)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameEnglishMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[A-Za-z\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameEnglishMustNotEmpty] ?? "ERROR Localizer");



            RuleFor(x => x)
                .Must(IsUniqueNameCombination)
                .WithMessage(_stringLocalizer[SharedResourceKeys.TheCombinationOfNameArAndNameEnMustBeUnique] ?? "ERROR Localizer");
            _stringLocalizer = stringLocalizer;
        }
        private bool IsUniqueNameCombination(PostSendToCommand command)
        {
            var repository = _unitOfWork.LK_Repository<LK_SendTo>();
            var existing = repository.GetAllAsTracking(x =>
            (x.NameAr == command.postSendToDto.NameAr || x.NameEn == command.postSendToDto.NameEn) &&
            x.State != State.Deleted).FirstOrDefault();

            return existing == null;
        }
    }
}
