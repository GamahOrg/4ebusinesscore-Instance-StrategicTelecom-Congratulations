namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Put
{
    public class PutSubCategoryCommandHandler(IResponseHelper _responseHelper, IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork _unitOfWork,
     IWebHostEnvironment _hosting, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<PutSubCategoryCommand, ResponseDTO>
    {
        public readonly IStringLocalizer<SharedResource> _stringLocalizer = stringLocalizer;
        public Guid LoggedInUser { get; set; } = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);
        public async Task<ResponseDTO> Handle(PutSubCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new PutSubCategoryValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
                return _responseHelper.FailedToSave(string.Join(" ", validation.Errors.Select(n => n.ErrorMessage).ToList()));

            var subCategory = _unitOfWork.Repository<Domain.Models.SubCatgeory>()
                .GetAllAsNoTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .FirstOrDefault();

            if (subCategory == null)
            {
                return _responseHelper.NotFound("SubCategoryNotFound");
            }
            var mainCategory = _unitOfWork.Repository<Domain.Models.Category>()
               .GetAllAsTracking(x => x.State != State.Deleted && x.Id == request.putSubCategoryDto.CategoryId)
               .Any();

            if (!mainCategory)
            {
                return _responseHelper.NotFound("TheMainCategoryNotFound");
            }

            subCategory.NameAr = request.putSubCategoryDto.NameAr;
            subCategory.NameEn = request.putSubCategoryDto.NameEn;
            subCategory.DescriptionEn = request.putSubCategoryDto.DescriptionEn;
            subCategory.DescriptionAr = request.putSubCategoryDto.DescriptionAr;
            subCategory.CategoryId = request.putSubCategoryDto.CategoryId;
            subCategory.UpdatedBy = LoggedInUser;
            subCategory.UpdatedOn = DateTime.UtcNow;

            await _unitOfWork.Repository<Domain.Models.SubCatgeory>().UpdateAsync(subCategory);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(subCategory.Id, _stringLocalizer[SharedResourceKeys.UpdateSuccessfully]);
        }
    }
}
