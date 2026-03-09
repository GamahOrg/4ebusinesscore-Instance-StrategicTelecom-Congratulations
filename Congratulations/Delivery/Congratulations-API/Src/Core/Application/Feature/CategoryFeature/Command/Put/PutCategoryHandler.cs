namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Put
{
    public class PutCategoryHandler : IRequestHandler<PutCategoryCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public PutCategoryHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }
        public async Task<ResponseDTO> Handle(PutCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new PutCategoryValidator(_unitOfWork, _stringLocalizer);
            var validation = validator.Validate(request);

            if (!validation.IsValid)
            {
                return _responseHelper.FailedToSave(string.Join(" ", validation.Errors.Select(n => n.ErrorMessage).ToList()));
            }
            var category = _unitOfWork.Repository<Category>()
                .GetAllAsTracking(x => x.Id == request.putCategoryDto.Id && x.State != State.Deleted)
                .FirstOrDefault();
            if (category == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            category.NameEn = request.putCategoryDto.NameEn;
            category.NameAr = request.putCategoryDto.NameAr;
            await _unitOfWork.Repository<Category>().UpdateAsync(category);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return _responseHelper.SavedSuccessfully(_stringLocalizer[SharedResourceKeys.UpdateSuccessfully]);

        }
    }
}
