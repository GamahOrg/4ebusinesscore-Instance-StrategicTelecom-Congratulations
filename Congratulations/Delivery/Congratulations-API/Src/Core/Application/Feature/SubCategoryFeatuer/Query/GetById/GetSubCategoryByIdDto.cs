namespace _4EPlatform.Application.Feature.SubCategoryFeature.Query.GetById
{
    public class GetSubCategoryByIdDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Guid CategoryId { get; set; }
    }
}
