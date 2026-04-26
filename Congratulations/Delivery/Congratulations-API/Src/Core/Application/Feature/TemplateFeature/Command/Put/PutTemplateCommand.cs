namespace _4EPlatform.Application.Feature.TemplateFeature.Command.Put
{
    public class PutTemplateCommand : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
        public PutTemplateDto PutTemplateDto { get; set; }
    }
}
