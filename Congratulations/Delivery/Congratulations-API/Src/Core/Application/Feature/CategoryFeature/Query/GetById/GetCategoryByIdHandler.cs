namespace _4EPlatform.Application.Feature.CategoryFeature.Query.GetById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public GetCategoryByIdHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }
        public async Task<ResponseDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category =await _unitOfWork.Repository<Category>()
                .GetAllAsNoTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .Select(x => new GetCategoryByIdDTO(x.NameAr, x.NameEn))
                .FirstOrDefaultAsync();
            if (category is null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            return _responseHelper.RetrievedSuccessfully(category, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully]);

        }
    }
}
