
namespace _4EPlatform.CongratsManagement.API.Controllers.V1.SubCategory
{
    public class SubCategoryController(
        IMediator _mediator) : BaseController
    {
        #region post 
        [HttpPost("PostSubCategory")]
        public async Task<ResponseDTO> PostSubCategory([FromForm] PostSubCategoryDto command)

            => await _mediator.Send(new PostSubCategoryCommand { postSubCategoryDto = command });

        #endregion
        #region Put
        [HttpPut("PutSubCategory/{Id}")]
        public async Task<ResponseDTO> PutSubCategory(Guid Id, [FromForm] PutSubCategoryDto command)

            => await _mediator.Send(new PutSubCategoryCommand
            {
                Id = Id,
                putSubCategoryDto = command
            });

        #endregion
        #region delete 
        [HttpDelete("DeleteSubCategory/{Id}")]
        public async Task<ResponseDTO> DeleteSubCategory(Guid Id)

            => await _mediator.Send(new DeleteSubCategoryCommand { Id = Id });

        #endregion
        #region Get
        [HttpGet("GetSubCategoryById/{Id}")]
        public async Task<ResponseDTO> GetSubCategoryById(Guid Id)

            => await _mediator.Send(new GetSubCategoryByIdQuery { Id = Id });


        [HttpGet("GetAllSubCategoryByMainCategoryId/{MainCategoryId}")]
        public async Task<ResponseDTO> GetAllSubCategoryByMainCategoryId(Guid MainCategoryId, bool? myCongrats, string? search, int PageNumber, int PageSize , Guid? SubCategoryId)

            => await _mediator.Send(new GetAllSubCategoryByMainCategoryIdQuery { MainCategoryId = MainCategoryId, MyCongrats = myCongrats, Search = search, PageNumber = PageNumber, PageSize = PageSize , SubCategoryId = SubCategoryId });

        #endregion
    }
}
