namespace _4EPlatform.Application.Feature.TemplateFeature.Query.GetAllBySubCategoryId
{
    public record GetAllTemplatesBySubCategoryIdQuery : IRequest<ResponseDTO>
    {
        public Guid SubCategoryId { get; set; }
    }
}
