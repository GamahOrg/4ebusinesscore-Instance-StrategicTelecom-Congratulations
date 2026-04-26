

namespace _4EPlatform.Application.Mapping
{
    public partial class MappingProfile
    {
        public void GroupsMapper()
        {
            CreateMap<PostGroupDto, Groups>();
            CreateMap<GroupDto, GroupProtoModel>()
              .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id.ToString() ?? ""))
              .ForMember(des => des.NameAr, opt => opt.MapFrom(src => src.NameAr ?? ""))
              .ForMember(des => des.NameEn, opt => opt.MapFrom(src => src.NameEn ?? ""))
              .ForMember(des => des.EmployeesProtoModels, opt => opt.MapFrom(src => src.Employees.Select(x => new EmployeesProtoModels() { UserId = x.ToString() })));

            CreateMap<GetAllGroupsDto, GetAllGroupsProtoModel>()
             .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id.ToString() ?? ""))
             .ForMember(des => des.NameAr, opt => opt.MapFrom(src => src.NameAr ?? ""))
             .ForMember(des => des.NameEn, opt => opt.MapFrom(src => src.NameEn ?? ""))
             .ForMember(des => des.CountOfEmployees, opt => opt.MapFrom(src => src.CountOfEmployees))
             .ForMember(des => des.EmployeesProtoModels, opt => opt.MapFrom(src => src.Employess.Select(x => new EmployeesProtoModels() { UserId = x.ToString() })));

        }
    }
}
