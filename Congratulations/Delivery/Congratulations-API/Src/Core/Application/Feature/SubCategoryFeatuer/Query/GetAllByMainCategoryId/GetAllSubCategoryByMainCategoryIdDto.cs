namespace _4EPlatform.Application.Feature.SubCategoryFeature.Query.GetAllByMainCategoryId
{
    public class GetAllSubCategoryByMainCategoryIdDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string CategoryNameAr { get; set; }
        public string CategoryNameEn { get; set; }
        public Guid MainCategoryId { get; set; }


        public int CountOfTemplates { get; set; }
    }
}
