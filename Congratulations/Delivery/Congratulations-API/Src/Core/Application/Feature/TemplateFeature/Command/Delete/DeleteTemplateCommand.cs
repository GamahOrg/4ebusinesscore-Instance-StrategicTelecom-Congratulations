namespace _4EPlatform.Application.Feature.TemplateFeature.Command.Delete
{
    public class DeleteTemplateCommand : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
    }
}
