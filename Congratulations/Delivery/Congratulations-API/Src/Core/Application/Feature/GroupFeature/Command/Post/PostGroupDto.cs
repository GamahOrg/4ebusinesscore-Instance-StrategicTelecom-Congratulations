

namespace _4EPlatform.Application.Feature.GroupFeature.Command.Post
{
    public record PostGroupDto
    {
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameAr { get; init; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameEn { get; init; }
        public required List<Guid> EmployeesGroupsIds { get; init; }
    }
}
