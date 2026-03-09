


namespace _4EPlatform.Application.Mapping
{
    public partial class MappingProfile
    {
        public void CongratsMapper()
        {
            CreateMap<PostCongratsDto, Domain.Models.Congrats>();
            CreateMap<PutCongratsDto, Domain.Models.Congrats>();
        }
    }
}
