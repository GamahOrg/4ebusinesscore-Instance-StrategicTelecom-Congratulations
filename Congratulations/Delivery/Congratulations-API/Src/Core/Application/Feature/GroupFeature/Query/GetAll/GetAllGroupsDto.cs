

namespace _4EPlatform.Application.Feature.GroupFeature.Query.GetAll
{
    public class GetAllGroupsDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public List<Guid> Employess { get; set; }
        public int? CountOfEmployees { get; set; }
    }
}
