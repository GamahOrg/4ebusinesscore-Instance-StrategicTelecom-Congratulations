

namespace _4EPlatform.CongratsManagement.API.Controllers.V1.Congrats
{

    public class CongratsController(IMediator _mediator)
        : BaseController
    {

        #region post
        [HttpPost("PostCongrats")]
        public async Task<ResponseDTO> PostCongrats([FromForm] PostCongratsDto postCongratsDto)
            => await _mediator.Send(new PostCongratsCommand { postCongratsDto = postCongratsDto });

        #endregion

        #region delete
        [HttpDelete("DeleteCongrats/{Id}")]
        public async Task<ResponseDTO> DeleteCongrats(Guid Id)
            => await _mediator.Send(new DeleteCongratsCommand { Id = Id });

        #endregion

        #region put
        [HttpPut("PutCongrats")]
        public async Task<ResponseDTO> PutCongrats([FromForm] PutCongratsDto command)

            => await _mediator.Send(new PutCongratsCommand { putCongratsDto = command });
        #endregion
    }
}
