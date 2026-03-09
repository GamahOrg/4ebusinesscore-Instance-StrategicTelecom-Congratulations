using Congrats.Application.Helpers.Helpers.QueryableExtensions;

namespace _4EPlatform.Application.Feature.SubCategoryFeature.Query.GetAllByMainCategoryId
{
    public class GetAllSubCategoryByMainCategoryIdQueryHandler(IUnitOfWork _unitOfWork, IStringLocalizer<SharedResource> stringLocalizer, IResponseHelper _responseHelper, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<GetAllSubCategoryByMainCategoryIdQuery, ResponseDTO>
    {
        IStringLocalizer<SharedResource> _stringLocalizer = stringLocalizer;
        public Guid LoggedInUserId { get; set; } = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);
        private ResponseDTO _responseDTO = new ResponseDTO();
        public async Task<ResponseDTO> Handle(GetAllSubCategoryByMainCategoryIdQuery request, CancellationToken cancellationToken)
        {

            var mainCategory = await _unitOfWork.Repository<Category>()
                .GetAllAsTracking(x => x.Id == request.MainCategoryId && x.State != State.Deleted)
                .FirstOrDefaultAsync();
            if (mainCategory == null)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.TheMainCategoryNotFound]);
            }
            var date = DateTime.UtcNow;
            DateTime utcNow = DateTime.UtcNow;

            // Get Saudi Arabia timezone
            TimeZoneInfo SaudiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

            // Convert to Saudi time
            DateTime SaudiNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, SaudiTimeZone);
            var congrats = _unitOfWork.Repository<Domain.Models.Congrats>().GetAllAsNoTracking(x =>
            x.State != State.Deleted &&
            !x.IsStopped &&
             (x.EndDate == null || x.EndDate > SaudiNow) &&
             x.SendDate <= SaudiNow &&
            (
                 x.UsersCongrats.Any(uc => uc.UserId == LoggedInUserId && uc.State != State.Deleted) ||
                 x.LK_SendToId == 1 ||   // جميع الإدارات
                 x.GroupsCongrats.Any(gc => gc.Groups.EmployeesGroups.Any(eg => eg.UserId == LoggedInUserId && eg.State != State.Deleted))
             )).Select(x => new { SubCatgeoryId = x.Template.SubCatgeoryId, TemplateId = x.TemplateId }).ToList();




            #region Filter
            var filterConditions = new Dictionary<Expression<Func<SubCatgeory, bool>>, bool>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                filterConditions.Add(x => x.NameAr == request.Search ||
                                          x.NameAr.Contains(request.Search) || x.NameEn == request.Search || x.NameEn.Contains(request.Search)
                                          , true);
            }

            #endregion




            var subCategory = _unitOfWork.Repository<Domain.Models.SubCatgeory>()
                .GetAllAsTracking(x => x.CategoryId == request.MainCategoryId && (request.SubCategoryId != null ? x.Id == request.SubCategoryId : true) && x.State != State.Deleted &&
                  (
                  (request.MyCongrats == null || request.MyCongrats == false) ||
                  (request.MyCongrats == true && congrats.Any() && (congrats != null ? congrats.Select(x => x.SubCatgeoryId).Contains(x.Id) : false))), request.PageNumber, request.PageSize, ref _responseDTO, x => x.CreatedOn, true)

                .ApplyFilters(filterConditions, request.PageNumber, request.PageSize, ref _responseDTO)

                .ToList().Select(x => new GetAllSubCategoryByMainCategoryIdDto
                {
                    Id = x.Id,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    MainCategoryId = x.CategoryId,
                    CategoryNameAr = x.Category.NameAr,
                    CategoryNameEn = x.Category.NameEn,
                    CountOfTemplates = request.MyCongrats != true ? x.Templates.Count(x => x.State != State.Deleted) : congrats.Where(co => co.SubCatgeoryId == x.Id).Count()

                })
                .ToList();
            if (subCategory.Count() <= 0)
            {
                return _responseHelper.NotFound(_stringLocalizer[SharedResourceKeys.NotFound]);
            }

            return _responseHelper.RetrievedSuccessfully(subCategory, _stringLocalizer[SharedResourceKeys.RetrievedSuccessfully], ref _responseDTO);
        }
    }
}
