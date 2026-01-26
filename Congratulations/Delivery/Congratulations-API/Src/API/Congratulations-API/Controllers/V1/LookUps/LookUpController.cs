namespace _4EPlatform.CongratsManagement.API.Controllers.V1.LookUps
{
    public class LookUpController(IMediator _mediator)
        : BaseController
    {
        [HttpPost("PostLookupSendTo")]
        public async Task<ResponseDTO> PostLookupSendTo([FromForm] PostSendToDto command)

            => await _mediator.Send(new PostSendToCommand { postSendToDto = command });

        [HttpGet("GetAllSendToLookUp")]
        public async Task<ResponseDTO> GetAllSendToLookUp()

            => await _mediator.Send(new GetAllSendToQuery());

        [HttpGet("GetSendToById/{Id}")]
        public async Task<ResponseDTO> GetSendToById(int Id)

            => await _mediator.Send(new GetSendToByIdQuery(Id));

        [HttpPut]
        public async Task<ResponseDTO> PutSendTo([FromForm] PutSendToDto command)

            => await _mediator.Send(new PutSendToCommand { putSendToDto = command });

        [HttpDelete("DeleteSendTo{Id}")]
        public async Task<ResponseDTO> DeleteSendTo(int Id)

            => await _mediator.Send(new DeleteSendToCommand(Id));

    }
}
