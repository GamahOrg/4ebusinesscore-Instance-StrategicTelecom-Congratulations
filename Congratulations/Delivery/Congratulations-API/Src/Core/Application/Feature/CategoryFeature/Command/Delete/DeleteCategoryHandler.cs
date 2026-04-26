namespace _4EPlatform.Application.Feature.CategoryFeature.Command.Delete
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public DeleteCategoryHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
        }
        public async Task<ResponseDTO> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _unitOfWork.Repository<Category>()
                .GetAllAsTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .FirstOrDefault();
            if (category is null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            if (category.SubCatgeory
                                .SelectMany(sc => sc.Templates)
                                .Any(t => _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsTracking().Any(c => c.TemplateId == t.Id)))
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.HaveCongrats]);
            }

            category.State = State.Deleted;
            await _unitOfWork.Repository<Category>().UpdateAsync(category);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return _responseHelper.SavedSuccessfully(_stringLocalizer[SharedResourceKeys.DeleteSuccessfully]);
        }
    }
}
