namespace _4EPlatform.Application.Feature.SubCategoryFeature.Query.GetAllByMainCategoryId
{
    public class GetAllSubCategoryByMainCategoryIdQuery : IRequest<ResponseDTO>
    {
        public Guid MainCategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }

        public bool? MyCongrats { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
