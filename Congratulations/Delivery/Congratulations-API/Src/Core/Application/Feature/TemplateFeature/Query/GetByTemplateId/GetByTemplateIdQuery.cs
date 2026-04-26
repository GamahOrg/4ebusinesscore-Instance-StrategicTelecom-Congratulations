namespace _4EPlatform.Application.Feature.TemplateFeature.Query.GetByTemplateId
{
    public class GetByTemplateIdQuery : IRequest<ResponseDTO>
    {
        public Guid Id { get; set; }
    }
}
