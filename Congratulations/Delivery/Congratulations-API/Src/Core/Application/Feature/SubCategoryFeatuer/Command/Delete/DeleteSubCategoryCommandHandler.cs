using _4EPlatform.Domain.Models;

namespace _4EPlatform.Application.Feature.SubCategoryFeature.Command.Delete
{
    public class DeleteSubCategoryCommandHandler : IRequestHandler<DeleteSubCategoryCommand, ResponseDTO>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IResponseHelper _responseHelper;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public DeleteSubCategoryCommandHandler(IUnitOfWork unitOfWork, IResponseHelper responseHelper, IStringLocalizer<SharedResource> stringLocalizer)

        {
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
            _stringLocalizer = stringLocalizer;


        }
        public async Task<ResponseDTO> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<SubCatgeory>()
                .GetAllAsTracking(x => x.Id == request.Id && x.State != State.Deleted)
                .FirstOrDefault();

            if (entity == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            if (entity .Templates
                    .Any(t => _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsTracking().Any(c => c.TemplateId == t.Id)))
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.HaveCongrats]);
            }
            entity.State = State.Deleted;

            await _unitOfWork.Repository<SubCatgeory>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(entity.Id, _stringLocalizer[SharedResourceKeys.DeleteSuccessfully]);

        }
    }
}
