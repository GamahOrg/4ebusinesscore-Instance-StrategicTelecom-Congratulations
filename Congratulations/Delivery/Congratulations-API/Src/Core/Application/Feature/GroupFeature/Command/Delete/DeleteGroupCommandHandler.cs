namespace _4EPlatform.Application.Feature.GroupFeature.Command.Delete
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, ResponseDTO>
    {
        private readonly IResponseHelper _responseHelper;
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public DeleteGroupCommandHandler(IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper responseHelper, IUnitOfWork unitOfWork)
        {
            _stringLocalizer = stringLocalizer;
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Groups>().GetAllAsNoTracking(x => x.State != State.Deleted && x.Id == request.Id).FirstOrDefault();
            if (entity == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            entity.State = State.Deleted;
            await _unitOfWork.Repository<Groups>().UpdateAsync(entity);

            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return _responseHelper.SavedSuccessfully(_stringLocalizer[SharedResourceKeys.DeleteSuccessfully]);
        }
    }
}
