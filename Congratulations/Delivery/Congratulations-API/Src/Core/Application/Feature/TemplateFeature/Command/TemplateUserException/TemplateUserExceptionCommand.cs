

namespace _4EPlatform.Application.Feature.TemplateFeature.Command.TemplateUserException
{
    public class TemplateUserExceptionCommand : IRequest<ResponseDTO>
    {
        //  public Guid UserId { get; set; }
        public string? NewUserName { get; set; }
        public string? NewJob { get; set; }
        public bool Job { get; set; }
        public Guid TemplateId { get; set; }
    }
}
