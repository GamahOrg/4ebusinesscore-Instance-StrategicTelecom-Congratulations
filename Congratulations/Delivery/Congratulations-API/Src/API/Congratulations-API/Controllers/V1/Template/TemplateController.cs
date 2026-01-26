
namespace _4EPlatform.CongratsManagement.API.Controllers.V1.Template
{
    public class TemplateController(
    IMediator _mediator)
        : BaseController
    {
        #region post 
        [HttpPost("PostTemplate")]
        public async Task<ResponseDTO> PostTemplate([FromForm] PostTemplateDto command)

            => await _mediator.Send(new PostTemplateCommand { postTemplateDto = command });

        #endregion
        #region Put
        [HttpPut("PutTemplate/{Id}")]
        public async Task<ResponseDTO> PutJudgmentGround(Guid Id, [FromForm] PutTemplateDto command)

            => await _mediator.Send(new PutTemplateCommand
            {
                Id = Id,
                PutTemplateDto = command
            });

        #endregion
        #region delete 
        [HttpDelete("DeleteTemplate/{Id}")]
        public async Task<ResponseDTO> DeleteTemplate(Guid Id)

            => await _mediator.Send(new DeleteTemplateCommand { Id = Id });

        #endregion
        #region Get
        [HttpGet("GetTemplateById/{Id}")]
        public async Task<ResponseDTO> GetTemplateById(Guid Id)

            => await _mediator.Send(new GetByTemplateIdQuery { Id = Id });

        [HttpGet("GetAllTemplatesBySubCategoryId/{SubCategoryId}")]
        public async Task<ResponseDTO> GetAllTemplatesBySubCategoryId(Guid SubCategoryId)

            => await _mediator.Send(new GetAllTemplatesBySubCategoryIdQuery { SubCategoryId = SubCategoryId });

        #endregion
        #region TemplateUserException
        [HttpPost("TemplateUserException")]
        public async Task<ResponseDTO> TemplateUserException([FromForm] TemplateUserExceptionCommand command)

            => await _mediator.Send(command);

        #endregion
    }
}
