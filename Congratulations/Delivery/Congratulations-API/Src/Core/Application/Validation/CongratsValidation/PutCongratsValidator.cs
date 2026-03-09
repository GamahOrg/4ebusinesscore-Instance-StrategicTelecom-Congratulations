namespace _4EPlatform.Application.Validation.CongratsValidation
{
    public class PutCongratsValidator : AbstractValidator<PutCongratsCommand>
    {
        private IUnitOfWork _unitOfWork;
        public PutCongratsValidator(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> _stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.putCongratsDto.SendDate)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.SendDateisrequired])
                .LessThan(x => x.putCongratsDto.EndDate ?? DateTime.MaxValue)
                .WithMessage(_stringLocalizer[SharedResourceKeys.SendDatemustbeearlierthanEndDate]);
            RuleFor(x => x.putCongratsDto)
           .Must(checkSendDate)
           .WithMessage(_stringLocalizer[SharedResourceKeys.SendDatemustbegreatethanDateTimeNow]);

            RuleFor(x => x.putCongratsDto.LK_SendToId)
              .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.LK_SendToisrequired]);

            RuleFor(x => x.putCongratsDto.TemplateId)
              .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Templateisrequired]);
        }

        private bool checkSendDate(PutCongratsDto dto)
        {
            DateTime utcNow = DateTime.UtcNow;

            // Get Saudi Arabia time zone
            TimeZoneInfo SaudiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

            // Convert to Saudi time
            DateTime SaudiNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, SaudiTimeZone);
            var date = _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsNoTracking(x => x.State != State.Deleted && x.Id == dto.Id).Select(x => x.SendDate).FirstOrDefault();

            if (date != null && date != dto.SendDate && dto.SendDate < SaudiNow)
            {
                return false;
            }
            return true;
        }
    }
}
