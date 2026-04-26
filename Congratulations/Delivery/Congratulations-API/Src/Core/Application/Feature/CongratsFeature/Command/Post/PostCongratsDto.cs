

namespace _4EPlatform.Application.Feature.CongratsFeature.Command.Post
{
    public record PostCongratsDto
    {
        public Guid TemplateId { get; init; }
        public DateTime SendDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool IsStopped { get; init; }
        public int LK_SendToId { get; init; }
        public List<Guid>? Groups { get; init; }
        public List<Guid>? Users { get; init; }
        public List<Guid>? Orgs { get; init; }
    }
}
