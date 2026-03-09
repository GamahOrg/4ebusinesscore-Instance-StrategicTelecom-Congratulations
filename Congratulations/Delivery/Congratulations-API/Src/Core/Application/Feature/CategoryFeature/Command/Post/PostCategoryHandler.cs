namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Post
{
    public class PostCategoryHandler : IRequestHandler<PostCategoryCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PostCategoryHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }

        public async Task<ResponseDTO> Handle(PostCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostCategoryValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
            {
                return _responseHelper.FailedToSave(string.Join(" ", validation.Errors.Select(n => n.ErrorMessage).ToList()));
            }
            var categoryAdd = new Category
            {
                NameAr = request.postCategoryDto.NameAr,
                NameEn = request.postCategoryDto.NameEn,
            };
            await _unitOfWork.Repository<Category>().AddAsync(categoryAdd);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return _responseHelper.SavedSuccessfully(categoryAdd.Id, _stringLocalizer[SharedResourceKeys.AddSuccessfully]);
        }
    }
}
