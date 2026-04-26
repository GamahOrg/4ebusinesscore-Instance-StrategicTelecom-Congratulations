namespace _4EPlatform.Application.Feature.TemplateFeature.Command.Post
{
    public class PostTemplateCommandHandler(
     IResponseHelper _responseHelper,
     IUnitOfWork _unitOfWork,
     IWebHostEnvironment _hosting,
      IStringLocalizer<SharedResource> _stringLocalizer,
     IHttpContextAccessor httpContextAccessor)
     : IRequestHandler<PostTemplateCommand, ResponseDTO>
    {
        public Guid LoggedInUserId { get; set; } = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);
        public async Task<ResponseDTO> Handle(PostTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = new Template
            {
                Name = request.postTemplateDto.Name,
                Text = request.postTemplateDto.Text ?? "",
                Description = request.postTemplateDto.Description,
                FontSize = request.postTemplateDto.FontSize,
                FontFamily = request.postTemplateDto.FontFamily,
                FontColor = request.postTemplateDto.FontColor,
                FontType = request.postTemplateDto.FontType,
                Job = request.postTemplateDto.Job,
                SubCatgeoryId = request.postTemplateDto.SubCategoryId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = LoggedInUserId
            };
            if (request.postTemplateDto.ImgUrl != null)
            {
                var filePath = await Upload.UploadFiles(request.postTemplateDto.ImgUrl, _hosting, "Template/" + Guid.NewGuid() + "/Images");

                template.ImageName = request.postTemplateDto.ImgUrl.FileName;
                template.ImageUrl = filePath;
            }
            await _unitOfWork.Repository<Template>().AddAsync(template);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(template.Id, _stringLocalizer[SharedResourceKeys.AddedSuccessfully]);
        }
    }
}
