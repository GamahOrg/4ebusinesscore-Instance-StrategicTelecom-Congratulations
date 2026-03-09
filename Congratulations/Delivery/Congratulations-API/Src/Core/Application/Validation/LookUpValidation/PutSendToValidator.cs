
namespace _4EPlatform.Application.Validation.CategoryValidation
{
    public class PutSendToValidator : AbstractValidator<PutSendToCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;
        public PutSendToValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.putSendToDto.NameAr)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameArabicMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[\u0600-\u06FF\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");



            RuleFor(x => x.putSendToDto.NameEn)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameEnglishMustNotEmpty] ?? "ERROR Localizer")
                .Matches(@"^[A-Za-z\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer");



            RuleFor(x => x)
                .Must(IsUniqueNameCombination)
                .WithMessage(_stringLocalizer[SharedResourceKeys.TheCombinationOfNameArAndNameEnMustBeUnique] ?? "ERROR Localizer");
            _stringLocalizer = stringLocalizer;
        }
        private bool IsUniqueNameCombination(PutSendToCommand command)
        {
            var repository = _unitOfWork.LK_Repository<LK_SendTo>();
            var existing = repository.GetAllAsTracking(x =>
            (x.NameAr == command.putSendToDto.NameAr || x.NameEn == command.putSendToDto.NameEn) &&
            x.State != State.Deleted
            && x.Id != command.putSendToDto.Id).FirstOrDefault();

            return existing == null;
        }
    }
}
