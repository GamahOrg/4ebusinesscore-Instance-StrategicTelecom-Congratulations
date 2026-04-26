namespace _4EPlatform.Application.Feature.TemplateFeature.Command.Put
{
    public class PutTemplateCommandHandler(
        IUnitOfWork _unitOfWork,
        IResponseHelper _responseHelper,
       IStringLocalizer<SharedResource> stringLocalizer,
    IWebHostEnvironment _hosting,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<PutTemplateCommand, ResponseDTO>
    {
        public readonly IStringLocalizer<SharedResource> _stringLocalizer = stringLocalizer;
        public Guid LoggedInUser { get; set; } = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);
        public async Task<ResponseDTO> Handle(PutTemplateCommand request, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Domain.Models.Template>()
                .GetAllAsTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .FirstOrDefault();
            if (entity == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }

            entity.Name = request.PutTemplateDto.Name;
            entity.Text = request.PutTemplateDto.Text ?? "";
            entity.Description = request.PutTemplateDto.Description;
            entity.FontFamily = request.PutTemplateDto.FontFamily;
            entity.FontType = request.PutTemplateDto.FontType;
            entity.FontColor = request.PutTemplateDto.FontColor;
            entity.FontSize = request.PutTemplateDto.FontSize;
            entity.Job = request.PutTemplateDto.Job;
            entity.SubCatgeoryId = request.PutTemplateDto.SubCategoryId;
            entity.UpdatedOn = DateTime.UtcNow;
            entity.UpdatedBy = LoggedInUser;

            if (request.PutTemplateDto.ImgUrl != null)
            {
                if (entity.ImageUrl != null)
                    Upload.DeleteFile(entity.ImageUrl);

                var filePath = await Upload.UploadFiles(request.PutTemplateDto.ImgUrl, _hosting, "Template/" + Guid.NewGuid() + "/Images");

                entity.ImageName = request.PutTemplateDto.ImgUrl.FileName;
                entity.ImageUrl = filePath;
            }

            await _unitOfWork.Repository<Domain.Models.Template>().UpdateAsync(entity);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(entity.Id, _stringLocalizer[SharedResourceKeys.UpdateSuccessfully]);
        }
    }
}
