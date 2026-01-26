


namespace _4EPlatform.CongratsManagement.API.Controllers.V1.Group
{
    public class GroupsController(IMediator _mediator)
        : BaseController
    {

        #region post
        [HttpPost("PostGroup")]
        public async Task<ResponseDTO> PostGroup([FromForm] PostGroupDto command)

            => await _mediator.Send(new PostGroupCommand { postGroupDto = command });

        #endregion

        #region put
        [HttpPut("PutGroup")]
        public async Task<ResponseDTO> PutGroup([FromForm] PutGroupDto command)

            => await _mediator.Send(new PutGroupCommand { putGroupDto = command });

        #endregion

        #region delete
        [HttpDelete("DeleteGroup/{Id}")]
        public async Task<ResponseDTO> DeleteGroup(Guid Id)

            => await _mediator.Send(new DeleteGroupCommand { Id = Id });

        #endregion

    }
}
