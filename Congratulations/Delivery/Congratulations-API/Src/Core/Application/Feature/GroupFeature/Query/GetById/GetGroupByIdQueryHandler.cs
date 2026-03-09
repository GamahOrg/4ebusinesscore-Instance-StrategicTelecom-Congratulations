

namespace _4EPlatform.Application.Feature.GroupFeature.Query.GetById
{
    public class GetGroupByIdQueryHandler(IUnitOfWork _unitOfWork, IResponseHelper _responseHelper)
        : IRequestHandler<GetGroupByIdQuery, ResponseDTO>
    {

        public async Task<ResponseDTO> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var entity =await _unitOfWork.Repository<Groups>().GetAllAsTracking(x => x.State != State.Deleted && x.Id == request.Id)
                 .Select(x =>
                 new GroupDto(
                     x.Id, x.NameAr, x.NameEn
                     , x.EmployeesGroups.Where(x => x.State != State.Deleted).Select(x => x.UserId)
                     .ToList()
                     ))
                 .FirstOrDefaultAsync();
            if (entity == null)
            {
                return _responseHelper.NotFound("Not Found");
            }
            return _responseHelper.RetrievedSuccessfully(entity, "Group Retrieved Successfully");
        }


    }
}
