namespace _4EPlatform.CongratsManagement.API.Controllers.V1.Categories
{
    public class CategoryController : BaseController
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("PostCategory")]
        public async Task<ResponseDTO> PostCategory([FromForm] PostCategoryDto postDto)

            => await _mediator.Send(new PostCategoryCommand { postCategoryDto = postDto });


        [HttpGet("GetAllCategories")]
        public async Task<ResponseDTO> GetAllCategories(bool? MyCongrats, string? Search, int PageSize, int PageNumber , Guid? CategoryId)

            => await _mediator.Send(new GetAllCategoryQuery(MyCongrats, Search, PageSize, PageNumber, CategoryId));

        [HttpGet("GetCategoryById/{Id}")]
        public async Task<ResponseDTO> GetCategoryById(Guid Id)

           => await _mediator.Send(new GetCategoryByIdQuery(Id));

        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<ResponseDTO> DeleteCategory(Guid Id)

            => await _mediator.Send(new DeleteCategoryCommand(Id));

        [HttpPut]
        public async Task<ResponseDTO> PutCategory([FromForm] PutCategoryDto putCategoryDto)

            => await _mediator.Send(new PutCategoryCommand { putCategoryDto = putCategoryDto });

    }
}
