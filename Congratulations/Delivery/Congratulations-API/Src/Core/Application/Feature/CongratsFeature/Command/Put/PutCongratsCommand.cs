

namespace _4EPlatform.Application.Feature.CongratsFeature.Command.Put
{
    public class PutCongratsCommand : IRequest<ResponseDTO>
    {

        public required PutCongratsDto putCongratsDto { get; set; }
    }


}
