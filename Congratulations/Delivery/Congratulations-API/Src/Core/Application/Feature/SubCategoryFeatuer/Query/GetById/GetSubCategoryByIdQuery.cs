namespace _4EPlatform.Application.Feature.SubCategoryFeature.Query.GetById
{
    public class GetSubCategoryByIdQuery : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
        public GetSubCategoryByIdDto GetSubCategoryByIdDto { get; set; }
    }
}
