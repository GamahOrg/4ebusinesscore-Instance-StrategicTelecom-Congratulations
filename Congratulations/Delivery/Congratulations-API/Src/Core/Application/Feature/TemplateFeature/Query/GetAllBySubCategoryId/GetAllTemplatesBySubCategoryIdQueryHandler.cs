namespace _4EPlatform.Application.Feature.TemplateFeature.Query.GetAllBySubCategoryId
{
    public class GetAllTemplatesBySubCategoryIdQueryHandler(IUnitOfWork _unitOfWork,
         IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper _responseHelper)
        : IRequestHandler<GetAllTemplatesBySubCategoryIdQuery, ResponseDTO>
    {
        public readonly IStringLocalizer<SharedResource> _stringLocalizer = stringLocalizer;
        public async Task<ResponseDTO> Handle(GetAllTemplatesBySubCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var subCategory = _unitOfWork.Repository<SubCatgeory>()
                .GetAllAsTracking(x => x.Id == request.SubCategoryId && x.State != State.Deleted)
                .FirstOrDefault();
            if (subCategory == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.SubCategoryNotFound]);
            }
            var templates = await _unitOfWork.Repository<Template>()
                .GetAllAsTracking(x => x.SubCatgeoryId == request.SubCategoryId && x.State != State.Deleted)
                .Select(x => new GetAllTemplatesBySubCategoryIdDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Text = x.Text ?? "",
                    Description = x.Description,
                    FontFamily = x.FontFamily,
                    FontType = x.FontType,
                    FontColor = x.FontColor,
                    FontSize = x.FontSize,
                    ImgName = x.ImageName ?? "",
                    ImgUrl = x.ImageUrl ?? "",
                    Job = x.Job,
                    SubCategoryId = request.SubCategoryId
                }).ToListAsync(cancellationToken);

            if (templates == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }

            return _responseHelper.RetrievedSuccessfully(templates, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully]);
        }
    }
}
