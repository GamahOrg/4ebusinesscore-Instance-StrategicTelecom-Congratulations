namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Post
{
    public class PostSendToCommand : IRequest<ResponseDTO>
    {
       public PostSendToDto postSendToDto {  get; set; }    
    }
}
