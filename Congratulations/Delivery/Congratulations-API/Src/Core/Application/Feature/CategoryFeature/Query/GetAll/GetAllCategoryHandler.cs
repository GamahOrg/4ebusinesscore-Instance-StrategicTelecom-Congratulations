namespace _4EPlatform.Application.Feature.CategoryFeature.Query.GetAll
{
    public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHelper _responseHelper;
        private readonly Guid LoggedInUserId;
        public readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private ResponseDTO _responseDTO;

        public GetAllCategoryHandler(IStringLocalizer<SharedResource> stringLocalizer, IUnitOfWork unitOfWork, IResponseHelper responseHelper, IHttpContextAccessor httpContextAccessor)
        {
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _responseHelper = responseHelper;
            LoggedInUserId = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);
            _responseDTO = new ResponseDTO();
        }
        public async Task<ResponseDTO> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {

            #region Filter
            var filterConditions = new Dictionary<Expression<Func<Category, bool>>, bool>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                filterConditions.Add(x => x.NameAr == request.Search ||
                                          x.NameAr.Contains(request.Search) || x.NameEn == request.Search || x.NameEn.Contains(request.Search)
                                          , true);
            }

            #endregion



            var date = DateTime.UtcNow;
            DateTime utcNow = DateTime.UtcNow;

            // Get Saudi Arabia timezone
            TimeZoneInfo SaudiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

            // Convert to Saudi time
            DateTime SaudiNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, SaudiTimeZone);
            var congrats = _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsTracking(x => (x.State != State.Deleted &&
                      x.Template.State != State.Deleted &&
                      x.Template.SubCatgeory.State != State.Deleted &&
                     x.Template.SubCatgeory.Category.State != State.Deleted &&
            !x.IsStopped &&
                    (x.EndDate == null || x.EndDate > SaudiNow) &&
                    x.SendDate <= SaudiNow &&
                    (
                        x.UsersCongrats.Any(uc => uc.UserId == LoggedInUserId && uc.State != State.Deleted) ||
                        x.LK_SendToId == 1 ||   // جميع الإدارات
                        x.GroupsCongrats.Any(gc => gc.Groups.EmployeesGroups.Any(eg => eg.UserId == LoggedInUserId && eg.State != State.Deleted))
                    ))).Select(x => new { CategoryId = x.Template.SubCatgeory.CategoryId, SubCatgeoryId = x.Template.SubCatgeoryId }).Distinct().ToList();

            var categories = await _unitOfWork.Repository<Category>()
                .GetAllAsNoTracking(x => x.State != State.Deleted &&
                (request.CategoryId != null ? x.Id == request.CategoryId : true)
                && (string.IsNullOrEmpty(request.Search) || x.NameAr.Contains(request.Search) || x.NameEn.Contains(request.Search))
                &&
                (
                (request.MyCongrats == null || request.MyCongrats == false ||
                 (request.MyCongrats == true && congrats.Any() && (congrats != null ? congrats.Select(x => x.CategoryId).Contains(x.Id) : false))))
                 , request.PageNumber, request.PageSize, ref _responseDTO)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new GetAllCategoryDTO(
                     x.Id,
                     x.NameAr,
                     x.NameEn,
                     x.SubCatgeory.Count(sc => sc.State != State.Deleted
                    && ((request.MyCongrats == null || request.MyCongrats == false) || (request.MyCongrats == true && congrats.Any() && (congrats != null ? congrats.Select(x => x.SubCatgeoryId).Contains(sc.Id) : false))
                     ))))
                 .ToListAsync(cancellationToken);

            if (!categories.Any())
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }
            return _responseHelper.RetrievedSuccessfully(categories, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully], ref _responseDTO);
        }
    }
}
