namespace _4EPlatform.Application.Feature.SubCategoryFeature.Query.GetById
{
    public class GetSubCategoryByIdQueryHandler(IUnitOfWork _unitOfWork, IStringLocalizer<SharedResource> stringLocalizer,

    IResponseHelper _responseHelper)
        : IRequestHandler<GetSubCategoryByIdQuery, ResponseDTO>
    {
        public readonly IStringLocalizer<SharedResource> _stringLocalizer = stringLocalizer;

        public async Task<ResponseDTO> Handle(GetSubCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var subCategory = _unitOfWork.Repository<Domain.Models.SubCatgeory>()
                .GetAllAsNoTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .Select(x => new GetSubCategoryByIdDto
                {
                    Id = request.Id,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    DescriptionAr = x.DescriptionAr ?? "",
                    DescriptionEn = x.DescriptionEn ?? "",
                    CategoryId = x.CategoryId,
                })
                .FirstOrDefault();

            if (subCategory == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            return _responseHelper.RetrievedSuccessfully(subCategory, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully]);
        }
    }
}
