namespace _4EPlatform.Application.Validation.CongratsValidation
{
    public class PostCongratsValidator : AbstractValidator<PostCongratsCommand>
    {

        public PostCongratsValidator(IStringLocalizer<SharedResource> _stringLocalizer)
        {
            RuleFor(x => x.postCongratsDto.SendDate)
      .Must(sendDate =>
          {
              var saudiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

              var saudiNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, saudiTimeZone);
              var sendDateSaudi = TimeZoneInfo.ConvertTimeFromUtc(
                sendDate.ToUniversalTime(), saudiTimeZone);

              return sendDateSaudi > saudiNow;
          })
.WithMessage(_stringLocalizer[SharedResourceKeys.SendDatemustbegreatethanDateTimeNow]);

            RuleFor(x => x.postCongratsDto.SendDate)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.SendDateisrequired] ?? "ERROR Localizer")
               .LessThan(x => x.postCongratsDto.EndDate ?? DateTime.MaxValue)
               .WithMessage(_stringLocalizer[SharedResourceKeys.SendDatemustbeearlierthanEndDate] ?? "ERROR Localizer");

            RuleFor(x => x.postCongratsDto.LK_SendToId)
              .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.LK_SendToisrequired] ?? "ERROR Localizer");

            RuleFor(x => x.postCongratsDto.TemplateId)
              .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Templateisrequired] ?? "ERROR Localizer");


        }




    }
}
