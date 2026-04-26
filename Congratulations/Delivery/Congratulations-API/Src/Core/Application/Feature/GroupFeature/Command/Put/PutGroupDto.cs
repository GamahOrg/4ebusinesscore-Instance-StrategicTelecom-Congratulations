

namespace _4EPlatform.Application.Feature.GroupFeature.Command.Put
{
    public record PutGroupDto
    {
        public Guid Id { get; init; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameAr { get; init; }
        [PreventInjectionHTML]
        [PreventInjectionCSV]
        [PreventInjectionScript]
        public required string NameEn { get; init; }
        public List<Guid>? EmployeesGroupsIds { get; init; }
    }

}
