namespace _4EPlatform.Application.Feature.TemplateFeature.Query.GetByTemplateId
{
    public class GetByTemplateIdQueryHandler(IUnitOfWork _unitOfWork, IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper _responseHelper)
        : IRequestHandler<GetByTemplateIdQuery, ResponseDTO>
    {
        public readonly IStringLocalizer<SharedResource> _stringLocalizer = stringLocalizer;
        public async Task<ResponseDTO> Handle(GetByTemplateIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Template>()
                .GetAllAsTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .Select(x => new GetByTemplateIdDto()
                {
                    Id = request.Id,
                    Name = x.Name ?? "",
                    Text = x.Text ?? "",
                    Description = x.Description ?? "",
                    FontFamily = x.FontFamily ?? "",
                    FontType = x.FontType ?? "",
                    FontColor = x.FontColor ?? "",
                    FontSize = x.FontSize,
                    ImgName = x.ImageName ?? "",
                    ImgUrl = x.ImageUrl ?? "",
                    Job = x.Job,
                    SubCategoryId = x.SubCatgeoryId
                })
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }

            return _responseHelper.RetrievedSuccessfully(entity, stringLocalizer[SharedResourceKeys.RetrievedSuccessfully]);
        }
    }
}
