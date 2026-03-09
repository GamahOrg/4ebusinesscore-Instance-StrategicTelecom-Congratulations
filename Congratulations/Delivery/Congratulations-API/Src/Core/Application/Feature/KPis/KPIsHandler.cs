namespace _4EPlatform.Application.Feature.KPis
{
    public class KPIsHandler : IRequestHandler<KPIsQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;

        public readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private ResponseDTO _responseDTO;

        public KPIsHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper, IHttpContextAccessor httpContextAccessor)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;

            _responseDTO = new ResponseDTO();
        }
        public async Task<ResponseDTO> Handle(KPIsQuery request, CancellationToken cancellationToken)
        {
            var congratCount = _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsNoTracking(x => x.State != State.NotDeleted).Count();
            var congratCountGroup = _unitOfWork.Repository<GroupsCongrats>().GetAllAsNoTracking(x => x.State != State.Deleted).Count();
            var congratCountOrgs = _unitOfWork.Repository<OrgsCongrats>().GetAllAsNoTracking(x => x.State != State.Deleted).Count();
            var congratCountUsers = _unitOfWork.Repository<UsersCongrats>().GetAllAsNoTracking(x => x.State != State.Deleted).Count();
            var congratTypes = _unitOfWork.LK_Repository<LK_SendTo>().GetAllAsNoTracking(x => x.State != State.Deleted).Count();
            var templateUsed = _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsNoTracking(x => x.State != State.Deleted && x.Template != null).Count();
            var result = (
       from template in _unitOfWork.Repository<Template>().GetAllAsNoTracking(x => x.State != State.Deleted)
       join congrats in _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsNoTracking(c => c.State != State.Deleted)
           on template.Id equals congrats.TemplateId into congratsGroup
       select new TemplateWithCongrat
       {
           TemplateId = template.Id,
           TemplateName = template.Name,
           CongratCount = congratsGroup.Count()
       }
         ).ToListAsync(cancellationToken);

            var dto = new KPIsDTO()
            {
                CountCongrats = congratCount,
                CongratForGroups = congratCountGroup,
                CongratForOrgs = congratCountOrgs,
                CongratForUsers = congratCountUsers,
                CongratTypes = congratTypes,
                templateWithCongrat = await result,
                UsedTemplate = templateUsed,

            };


            return _responseHelper.RetrievedSuccessfully(dto, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully], ref _responseDTO);
        }
    }

}

