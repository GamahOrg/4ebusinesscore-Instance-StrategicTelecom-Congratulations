
using _4EPlatform.Application.Feature.GroupFeature.Command.Put;

namespace _4EPlatform.Application.Validation.GroupsValidation
{
    public class PutGroupValidator : AbstractValidator<PutGroupCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PutGroupValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;

            RuleFor(x => x.putGroupDto.NameAr)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameArabicMustNotEmpty] ?? "ERROR Localizer")
               .Matches(@"^[\u0600-\u06FF\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInArabicMustBeWrittenInArabicAAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x.putGroupDto.NameEn)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NameInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer")
                .Matches(@"^[A-Za-z\s0-9._]+$").WithMessage(_stringLocalizer[SharedResourceKeys.NameInEnglishMustBeWrittenInEnglishAndCanContainNumbers] ?? "ERROR Localizer");

            RuleFor(x => x)
             .Must(IsUniqueNameCombination)
             .WithMessage(_stringLocalizer[SharedResourceKeys.TheCombinationOfNameArAndNameEnMustBeUnique] ?? "ERROR Localizer");
            _stringLocalizer = stringLocalizer;
        }

        private bool IsUniqueNameCombination(PutGroupCommand command)
        {
            var repository = _unitOfWork.Repository<Domain.Models.Groups>();
            var existing = repository.GetAllAsTracking(x =>
            (x.NameAr == command.putGroupDto.NameAr || x.NameEn == command.putGroupDto.NameEn) &&
            x.State != State.Deleted && x.Id != command.putGroupDto.Id).FirstOrDefault();

            return existing == null;
        }
    }
}
