namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Post
{

    public class PostSubCategoryCommandHandler(
   IResponseHelper _responseHelper,
   IUnitOfWork _unitOfWork,

   IHttpContextAccessor httpContextAccessor, IStringLocalizer<SharedResource> stringLocalizer)
   : IRequestHandler<PostSubCategoryCommand, ResponseDTO>
    {
        public readonly IStringLocalizer<SharedResource> _stringLocalizer = stringLocalizer;
        public Guid LoggedInUser { get; set; } = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);

        public async Task<ResponseDTO> Handle(PostSubCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostSubCategoryValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
                return _responseHelper.FailedToSave(string.Join(" ", validation.Errors.Select(n => n.ErrorMessage).ToList()));

            var mainCategory = _unitOfWork.Repository<Category>()
                .GetAllAsTracking(x => x.State != State.Deleted && x.Id == request.postSubCategoryDto.CategoryId)
                .Any();

            if (!mainCategory)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.TheMainCategoryNotFound]);
            }

            var subCategory = new SubCatgeory
            {
                NameAr = request.postSubCategoryDto.NameAr,
                NameEn = request.postSubCategoryDto.NameEn,
                DescriptionAr = request.postSubCategoryDto.DescriptionAr,
                DescriptionEn = request.postSubCategoryDto.DescriptionEn,
                CategoryId = request.postSubCategoryDto.CategoryId,
                CreatedBy = LoggedInUser,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.Repository<SubCatgeory>().UpdateAsync(subCategory);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(subCategory.Id, _stringLocalizer[SharedResourceKeys.AddedSuccessfully]);
        }
    }
}


