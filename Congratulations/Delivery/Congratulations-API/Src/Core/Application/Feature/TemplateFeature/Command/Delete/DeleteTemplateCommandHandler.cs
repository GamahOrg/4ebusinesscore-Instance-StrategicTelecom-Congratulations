using _4EPlatform.Domain.Models;

namespace _4EPlatform.Application.Feature.TemplateFeature.Command.Delete
{
    public class DeleteTemplateCommandHandler( IUnitOfWork _unitOfWork,IResponseHelper _responseHelper)
        : IRequestHandler<DeleteTemplateCommand, ResponseDTO>
    {
        public async Task<ResponseDTO> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Domain.Models.Template>()
                .GetAllAsTracking(x=> x.Id == request.Id && x.State != State.Deleted )
                .FirstOrDefault();

            if (entity == null)
            {
                return _responseHelper.NotFound("TemplateNotFound");
            }
            if (_unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsNoTracking(x=>x.TemplateId==request.Id).Any())
            {
                return _responseHelper.NotFound("Have Congrats");
            }

            entity.State = State.Deleted;
            await _unitOfWork.Repository<Template>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _responseHelper.SavedSuccessfully(entity.Id, "TemplateDeletedSuccessfully");
        }
    }
}
