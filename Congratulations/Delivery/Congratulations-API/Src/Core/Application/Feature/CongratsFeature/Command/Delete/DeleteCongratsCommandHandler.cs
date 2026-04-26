namespace _4EPlatform.Application.Feature.CongratsFeature.Command.Delete
{
    public class DeleteCongratsCommandHandler : IRequestHandler<DeleteCongratsCommand, ResponseDTO>
    {
        private readonly IResponseHelper _responseHelper;
        private readonly IUnitOfWork _unitOfWork;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public DeleteCongratsCommandHandler(IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper responseHelper, IUnitOfWork unitOfWork)
        {
            _stringLocalizer = stringLocalizer;
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> Handle(DeleteCongratsCommand request, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsNoTracking(x => x.State != State.Deleted && x.Id == request.Id).FirstOrDefault();
            if (entity == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            entity.State = State.Deleted;
            await _unitOfWork.Repository<Domain.Models.Congrats>().UpdateAsync(entity);

            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return _responseHelper.SavedSuccessfully(_stringLocalizer[SharedResourceKeys.DeleteSuccessfully]);
        }
    }
}
