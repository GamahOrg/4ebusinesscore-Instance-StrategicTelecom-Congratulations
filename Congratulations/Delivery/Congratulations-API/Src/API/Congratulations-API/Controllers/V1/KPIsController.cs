

namespace Controllers.V1
{
    public class KPIsController(IMediator _mediator)
        : BaseController
    {

        [HttpGet]
        public async Task<ResponseDTO> Get()

        => await _mediator.Send(new KPIsQuery());


    }
}
