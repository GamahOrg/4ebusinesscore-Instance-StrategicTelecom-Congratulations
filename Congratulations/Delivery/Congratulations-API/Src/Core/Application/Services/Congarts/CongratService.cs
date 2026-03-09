

namespace _4EPlatform.Application.Services.Congarts
{
    public class CongratService(IUnitOfWork _unitOfWork, IHttpContextAccessor httpContextAccessor) : CongratsProtoService.CongratsProtoServiceBase
    {
        private ResponseDTO _responseDTO = new ResponseDTO();
        public Guid LoggedInUserId { get; set; } = LoggedInUserProvider.GetLoggedInUserId(httpContextAccessor);
        public override Task<GetCongratByIdResponse> GetGrouById(GetCongratByIdRequest request, ServerCallContext context)
        {
            var response = new GetCongratByIdResponse();
            try
            {
                var congrat = _unitOfWork.Repository<Domain.Models.Congrats>()
                    .GetAllAsTracking(x => x.Id == Guid.Parse(request.Id) && x.State != State.Deleted)
                    .FirstOrDefault();

                if (congrat == null)
                {
                    response.Message = "NotFound";
                    response.StatusEnum = (int)StatusEnum.Failed;
                    return Task.FromResult(response);
                }

                var entity = new CongratProtoModel()
                {
                    Id = congrat.Id.ToString(),
                    CategoryID = congrat.Template.SubCatgeory.CategoryId.ToString(),
                    SubCategoryID = congrat.Template.SubCatgeoryId.ToString(),
                    CategoryNameAr = congrat.Template.SubCatgeory.Category.NameAr ?? "",
                    CategoryNameEn = congrat.Template.SubCatgeory.Category.NameEn ?? "",
                    SubCategoryNameAr = congrat.Template.SubCatgeory.NameAr ?? "",
                    SubCategoryNameEn = congrat.Template.SubCatgeory.NameEn ?? "",
                    SendDate = congrat.SendDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? "",
                    EndDate = congrat.EndDate?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? "",
                    LkSendTo = congrat.LK_SendToId,
                    NameArLkSendTo = congrat.LK_SendTo.NameAr ?? "",
                    NameEnLkSendTo = congrat.LK_SendTo.NameEn ?? "",
                    CongratStatus = -1,
                    IsStopped = congrat.IsStopped,

                    TemplateProtoModels = new TemplateProtoModels
                    {
                        Id = congrat.Template.Id.ToString(),
                        Name = congrat.Template.Name ?? "",
                        Description = congrat.Template.Description ?? "",
                        FontFamily = congrat.Template.FontFamily ?? "",
                        FontType = congrat.Template.FontType ?? "",
                        FontColor = congrat.Template.FontColor ?? "",
                        FontSize = congrat.Template.FontSize,
                        ImageName = congrat.Template.ImageName ?? "",
                        ImageUrl = congrat.Template.ImageUrl ?? "",
                        Job = congrat.Template.Job,
                        Text = congrat.Template.Text ?? "Not Added",
                    }
                };

                entity?.UserIds.AddRange(congrat.UsersCongrats.Where(x => x.State != State.Deleted).Select(x => x.UserId.ToString())
                    .ToList());

                entity?.OrgIds.AddRange(congrat.OrgsCongrats.Where(x => x.State != State.Deleted).Select(x => x.OrgId.ToString())
                    .ToList());

                entity?.GroupProtoModels.AddRange(congrat.GroupsCongrats.Where(x => x.State != State.Deleted)
                    .ToList().Select(x =>
                    {
                        var group = new GroupProtoModelCongrat
                        {
                            GroupsIds = x.Groups.Id.ToString(),
                            GroupNameAr = x.Groups.NameAr ?? "",
                            GroupNameEn = x.Groups.NameEn ?? "",

                        };
                        group.UserIds.AddRange(x.Groups.EmployeesGroups.Where(x => x.State != State.Deleted).Select(x => x.UserId.ToString()).ToList());
                        return group;
                    }));


                response.CongratProtoModel = entity;
                response.Message = "Success";
                response.StatusEnum = (int)StatusEnum.Success;
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusEnum = (int)StatusEnum.Failed;
                return Task.FromResult(response);
            }
        }
        public async override Task<GetAllCongratsResponse> GetAllCongrats(GetAllCongratsRequest request, ServerCallContext context)
        {
            Guid? subCategoryGuid = null;
            Guid? categoryGuid = null;
            DateTime utcNow = DateTime.UtcNow;

            // Get Saudi Arabia timezone
            TimeZoneInfo saudiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

            // Convert to Saudi time
            DateTime saudiNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, saudiTimeZone);
            if (!string.IsNullOrEmpty(request.SubCategoryId) && Guid.TryParse(request.SubCategoryId, out var parsedGuid))
            {
                subCategoryGuid = parsedGuid;
            }
            if (!string.IsNullOrEmpty(request.CategoryId) && Guid.TryParse(request.CategoryId, out var ParseCategoryId))
            {
                categoryGuid = ParseCategoryId;
            }

            var allCongrats = _unitOfWork.Repository<Domain.Models.Congrats>()
                .GetAllAsTracking(x => x.State == State.NotDeleted &&
               (!subCategoryGuid.HasValue || x.Template.SubCatgeoryId == subCategoryGuid.Value) &&
               (!categoryGuid.HasValue || x.Template.SubCatgeory.CategoryId == categoryGuid.Value)
                &&
                (request.LkSendTo == 0 || x.LK_SendToId == request.LkSendTo) &&
                (
                (request.CongratStatus == -1 ||
                (request.CongratStatus == (int)CongratStatus.Permanent && (x.EndDate == null && x.IsStopped != true)) ||  // دائمة
                (request.CongratStatus == (int)CongratStatus.Temporary && x.EndDate != null && x.EndDate >= saudiNow && x.IsStopped == false) || // سارية
                (request.CongratStatus == (int)CongratStatus.Stopped && (x.EndDate != null && x.EndDate < saudiNow || x.IsStopped == true)))
                ) && // متوقفه
                (string.IsNullOrEmpty(request.EndDate) || (x.EndDate != null ? x.EndDate.Value.Date == DateTime.Parse(request.EndDate).Date : false)) &&
                (string.IsNullOrEmpty(request.SendDate) || x.SendDate.Date == DateTime.Parse(request.SendDate).Date) &&
                (string.IsNullOrEmpty(request.SendDate) || (DateTime.Parse(request.SendDate).TimeOfDay != TimeSpan.Zero ? x.SendDate.TimeOfDay == DateTime.Parse(request.SendDate).TimeOfDay : true)) &&
                (
                (string.IsNullOrEmpty(request.Search) ||
                x.Template.SubCatgeory.NameAr.Contains(request.Search) || x.Template.SubCatgeory.NameEn.Contains(request.Search) ||
                x.Template.SubCatgeory.Category.NameAr.Contains(request.Search) || x.Template.SubCatgeory.Category.NameEn.Contains(request.Search) ||
                x.Template.Name.Contains(request.Search)
                ))
                ,
                request.PageNumber, request.PageSize, ref _responseDTO, x => x.CreatedOn, true
                ).ToList();

            var response = new GetAllCongratsResponse();
            if (allCongrats == null)
            {
                response.Message = "NotFound";
                response.StatusEnum = (int)StatusEnum.Failed;
                return await Task.FromResult(response);
            }

            var entity = allCongrats.Select(congrat => new CongratProtoModel()
            {

                Id = congrat.Id.ToString(),
                CategoryNameAr = congrat.Template.SubCatgeory.Category.NameAr ?? "not fount",
                CategoryNameEn = congrat.Template.SubCatgeory.Category.NameEn ?? "not fount",
                SubCategoryNameAr = congrat.Template.SubCatgeory.NameAr ?? "",
                SubCategoryNameEn = congrat.Template.SubCatgeory.NameEn ?? "",
                SendDate = congrat.SendDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? "",
                EndDate = congrat.EndDate?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? "",
                LkSendTo = congrat.LK_SendToId,
                NameArLkSendTo = congrat.LK_SendTo.NameAr ?? "",
                NameEnLkSendTo = congrat.LK_SendTo.NameEn ?? "",
                IsStopped = congrat.IsStopped,
                TemplateProtoModels = new TemplateProtoModels
                {
                    Id = congrat.Template.Id.ToString(),
                    Name = congrat.Template.Name ?? "",
                    Description = congrat.Template.Description ?? "",
                    FontFamily = congrat.Template.FontFamily ?? "",
                    FontType = congrat.Template.FontType ?? "",
                    FontColor = congrat.Template.FontColor ?? "",
                    FontSize = congrat.Template.FontSize,
                    ImageName = congrat.Template.ImageName ?? "",
                    ImageUrl = congrat.Template.ImageUrl ?? "NotFound",
                    Job = congrat.Template.Job,
                    Text = congrat.Template.Text ?? "Not Added"
                }
            }).ToList();
            foreach (var congrat in entity)
            {
                congrat.UserIds.Add(allCongrats.Where(x => x.Id == Guid.Parse(congrat.Id))
                    .SelectMany(x => x.UsersCongrats.Where(x => x.State != State.Deleted))
                    .Select(x => x.UserId.ToString()));

                congrat.OrgIds.Add(allCongrats.Where(x => x.Id == Guid.Parse(congrat.Id))
                    .SelectMany(x => x.OrgsCongrats.Where(x => x.State != State.Deleted))
                    .Select(x => x.OrgId.ToString()));

                congrat.GroupProtoModels.AddRange(allCongrats.Where(x => x.Id == Guid.Parse(congrat.Id))
                    .SelectMany(x => x.GroupsCongrats.Where(x => x.State != State.Deleted))
                    .Select(x => new GroupProtoModelCongrat
                    {
                        GroupsIds = x.Groups.Id.ToString(),
                        GroupNameAr = x.Groups.NameAr ?? "",
                        GroupNameEn = x.Groups.NameEn ?? ""
                    }));

                if (congrat.EndDate == string.Empty)
                    congrat.CongratStatus = (int)CongratStatus.Permanent;
                if (congrat.EndDate != string.Empty && DateTime.Parse(congrat.EndDate) >= saudiNow && congrat.IsStopped == false)
                    congrat.CongratStatus = (int)CongratStatus.Temporary;
                if ((congrat.EndDate != string.Empty && DateTime.Parse(congrat.EndDate) < saudiNow)
                || congrat.IsStopped == true)
                    congrat.CongratStatus = (int)CongratStatus.Stopped;

            }

            response.CongratProtoModel.AddRange(entity);
            response.Message = "RetrievedSuccess";
            response.StatusEnum = (int)StatusEnum.Success;
            response.PageNumber = _responseDTO.PageIndex;
            response.PageSize = _responseDTO.PageSize;
            response.TotalItems = _responseDTO.TotalItems;
            response.TotalPages = _responseDTO.TotalPages;

            return await Task.FromResult(response);

        }
        public async override Task<GetAllMyCongratsResponse> GetAllMyCongrats(GetAllMyCongratsRequest request, ServerCallContext context)
        {
            DateTime utcNow = DateTime.UtcNow;

            // Get Saudi Arabia timezone
            TimeZoneInfo SaudiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

            // Convert to Saudi time
            DateTime SaudiNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, SaudiTimeZone);

            // Console.WriteLine("Current time in Saudi Arabia: " + saudiNow.ToString("yyyy-MM-dd HH:mm:ss"));
            var date = DateTime.UtcNow;
            var strdate = DateTime.Parse(date.ToString());
            var myCongrats = _unitOfWork.Repository<Domain.Models.Congrats>()
                .GetAllAsTracking(x =>
                    x.SendDate <= SaudiNow &&
                     x.State == State.NotDeleted &&
                     x.Template.State != State.Deleted &&
                     x.Template.SubCatgeory.State != State.Deleted &&
                     x.Template.SubCatgeory.Category.State != State.Deleted &&
                    !x.IsStopped &&

                    (x.EndDate == null || x.EndDate > SaudiNow) &&
                    (
                        x.UsersCongrats.Any(uc => uc.UserId == LoggedInUserId && uc.State != State.Deleted) ||
                        x.LK_SendToId == 1 ||   // جميع الإدارات
                        x.GroupsCongrats.Any(gc => gc.Groups.EmployeesGroups.Any(eg => eg.UserId == LoggedInUserId && eg.State != State.Deleted))
                    )
                    &&
                    (string.IsNullOrEmpty(request.SubCategoryId) || x.Template.SubCatgeoryId == Guid.Parse(request.SubCategoryId))
                    ,
                    request.PageNumber, request.PageSize, ref _responseDTO)
                .Select(congrat => new MyCongratProtoModel()
                {
                    Id = congrat.Id.ToString(),
                    TemplateProtoModels = new TemplateProtoModels
                    {
                        Id = congrat.Template.Id.ToString(),
                        Name = congrat.Template.Name ?? "",
                        Description = congrat.Template.Description ?? "",
                        FontFamily = congrat.Template.FontFamily ?? "",
                        FontType = congrat.Template.FontType ?? "",
                        FontColor = congrat.Template.FontColor ?? "",
                        FontSize = congrat.Template.FontSize,
                        ImageName = congrat.Template.ImageName ?? "",
                        ImageUrl = congrat.Template.ImageUrl ?? "",
                        CategoryID = congrat.Template.SubCatgeory.CategoryId.ToString() ?? "",
                        CategoryNameAr = congrat.Template.SubCatgeory.Category.NameAr ?? "",
                        CategoryNameEn = congrat.Template.SubCatgeory.Category.NameEn ?? "",
                        SubCategoryId = congrat.Template.SubCatgeoryId.ToString() ?? "",
                        SubCategoryNameAr = congrat.Template.SubCatgeory.NameAr ?? "",
                        SubCategoryNameEn = congrat.Template.SubCatgeory.NameEn ?? "",
                        Text = congrat.Template.Text ?? "",
                        Job = congrat.Template.Exceptions
                        .Where(x => x.UserId == LoggedInUserId && x.State != State.Deleted)
                        .FirstOrDefault() != null ? congrat.Template.Exceptions
                        .Where(x => x.UserId == LoggedInUserId && x.State != State.Deleted)
                        .Select(x => x.Job).FirstOrDefault() : congrat.Template.Job,
                        UserName = congrat.Template.Exceptions
                            .Where(x => x.UserId == LoggedInUserId && x.State != State.Deleted)
                            .Select(x => x.NewUserName)
                            .FirstOrDefault(),
                        JobName = congrat.Template.Exceptions
                            .Where(x => x.UserId == LoggedInUserId && x.State != State.Deleted)
                            .Select(x => x.NewJob)
                            .FirstOrDefault()

                    }
                }).ToList();

            var response = new GetAllMyCongratsResponse();
            if (myCongrats == null)
            {
                response.Message = "NotFound";
                response.StatusEnum = (int)StatusEnum.Failed;
                return await Task.FromResult(response);
            }


            response.MyCongratProtoModels.AddRange(myCongrats);
            response.Message = "RetrievedSuccess";
            response.StatusEnum = (int)StatusEnum.Success;
            response.PageNumber = _responseDTO.PageIndex;
            response.PageSize = _responseDTO.PageSize;
            response.TotalItems = _responseDTO.TotalItems;
            response.TotalPages = _responseDTO.TotalPages;

            return await Task.FromResult(response);
        }
    }
}
