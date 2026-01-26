
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace _4EPlatForm.Infrastructure.Context
{
    public class ContextSeed
    {
        private static AppDBContext? _dbContext;
        private static IServiceProvider _serviceProvider;
        private static IConfiguration? _configuration;
        public static void Seed(AppDBContext appDbContext, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _configuration = configuration;

            _dbContext = appDbContext;
            _dbContext.Database.EnsureCreated();
            _serviceProvider = serviceProvider;
            SeedSendTo();

            var defaultDataEnabled = _configuration.GetValue<bool>("IsSeedData");

            if (defaultDataEnabled)
            {
                Seed_Categories();
                Seed_SubCategories();
                Seed_Template();
                Group_SeedData();
                EmployeesGroups_SeedData();
                Congrats_SeedData();
                GroupsCongrats_SeedData();
                OrgsCongrats_SeedData();
                UsersCongrats_SeedData();
                TemplateUserException_SeedData();

            }

        }
        private static void Seed_Categories()
        {
            try
            {
                string CategoriesJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "Category_SeedData.json");
                var CategoriesJsonTypes = JsonConvert.DeserializeObject<List<Category>>(CategoriesJson);

                if (CategoriesJsonTypes != null && CategoriesJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.Category.Any())
                        {
                            foreach (var LK_IntegrationType in CategoriesJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.Category.AddRange(CategoriesJsonTypes);

                            _dbContext.SaveChanges();
                        }

                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {


            }
        }

        private static void Seed_SubCategories()
        {
            try
            {

                string SubCategoriesJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "SubCategory_SeedData.json");
                var SubCategoriesJsonTypes = JsonConvert.DeserializeObject<List<SubCatgeory>>(SubCategoriesJson);

                if (SubCategoriesJsonTypes != null && SubCategoriesJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.SubCatgeories.Any())
                        {
                            foreach (var LK_IntegrationType in SubCategoriesJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.SubCatgeories.AddRange(SubCategoriesJsonTypes);

                            _dbContext.SaveChanges();
                        }

                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {


            }
        }
        private static void Seed_Template()
        {
            try
            {

                string TemplatesJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "Template_SeedData.json");
                var TemplatesJsonTypes = JsonConvert.DeserializeObject<List<Template>>(TemplatesJson);

                if (TemplatesJsonTypes != null && TemplatesJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.Templates.Any())
                        {
                            foreach (var LK_IntegrationType in TemplatesJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.Templates.AddRange(TemplatesJsonTypes);

                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            var existingTemplatesTypes = _dbContext.Templates.ToList();
                            var newTemplates = TemplatesJsonTypes
                                .Where(na => !existingTemplatesTypes.Any(ea => ea.Id == na.Id))
                                .ToList();

                            if (newTemplates.Any())
                            {
                                foreach (var Template in newTemplates)
                                {
                                    Template.CreatedOn = DateTime.Now;
                                }

                                _dbContext.Templates.AddRange(newTemplates);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }

            catch
            {


            }
        }
        private static void Group_SeedData()
        {

            try
            {
                string GroupsJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "Group_SeedData.json");
                var GroupsJsonTypes = JsonConvert.DeserializeObject<List<Groups>>(GroupsJson);

                if (GroupsJsonTypes != null && GroupsJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.Groups.Any())
                        {
                            foreach (var LK_IntegrationType in GroupsJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.Groups.AddRange(GroupsJsonTypes);

                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            var existingGroupTypes = _dbContext.Groups.ToList();
                            var newGroups = GroupsJsonTypes
                                .Where(na => !existingGroupTypes.Any(ea => ea.Id == na.Id))
                                .ToList();

                            if (newGroups.Any())
                            {
                                foreach (var Group in newGroups)
                                {
                                    Group.CreatedOn = DateTime.Now;
                                }

                                _dbContext.Groups.AddRange(newGroups);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {


            }
        }
        private static void EmployeesGroups_SeedData()
        {
            try
            {

                string EmployeesGroupsJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "EmployeesGroups_SeedData.json");
                var EmployeesGroupsJsonTypes = JsonConvert.DeserializeObject<List<EmployeesGroups>>(EmployeesGroupsJson);

                if (EmployeesGroupsJsonTypes != null && EmployeesGroupsJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.EmployeesGroups.Any())
                        {
                            foreach (var LK_IntegrationType in EmployeesGroupsJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.EmployeesGroups.AddRange(EmployeesGroupsJsonTypes);

                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            var existingEmployeesGroupsTypes = _dbContext.EmployeesGroups.ToList();
                            var newEmployeesGroups = EmployeesGroupsJsonTypes
                                .Where(na => !existingEmployeesGroupsTypes.Any(ea => ea.Id == na.Id))
                                .ToList();

                            if (newEmployeesGroups.Any())
                            {
                                foreach (var newEmployeesGroup in newEmployeesGroups)
                                {
                                    newEmployeesGroup.CreatedOn = DateTime.Now;
                                }

                                _dbContext.EmployeesGroups.AddRange(newEmployeesGroups);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {


            }
        }
        private static void Congrats_SeedData()
        {
            try
            {
                string CongratsJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "Congrats_SeedData.json");
                var CongratsJsonTypes = JsonConvert.DeserializeObject<List<Congrats>>(CongratsJson);

                if (CongratsJsonTypes != null && CongratsJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.Congrats.Any())
                        {
                            foreach (var LK_IntegrationType in CongratsJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.Congrats.AddRange(CongratsJsonTypes);

                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            var existingCongratsTypes = _dbContext.Congrats.ToList();
                            var newCongrats = CongratsJsonTypes
                                .Where(na => !existingCongratsTypes.Any(ea => ea.Id == na.Id))
                                .ToList();

                            if (newCongrats.Any())
                            {
                                foreach (var Congrat in newCongrats)
                                {
                                    Congrat.CreatedOn = DateTime.Now;
                                }

                                _dbContext.Congrats.AddRange(CongratsJsonTypes);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {


            }
        }
        private static void GroupsCongrats_SeedData()
        {
            try
            {
                string GroupsCongratsJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "GroupsCongrats_SeedData.json");
                var GroupsCongratsTypes = JsonConvert.DeserializeObject<List<GroupsCongrats>>(GroupsCongratsJson);

                if (GroupsCongratsTypes != null && GroupsCongratsTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.GroupsCongrats.Any())
                        {
                            foreach (var LK_IntegrationType in GroupsCongratsTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.GroupsCongrats.AddRange(GroupsCongratsTypes);

                            _dbContext.SaveChanges();
                        }

                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {


            }
        }
        private static void OrgsCongrats_SeedData()
        {
            try
            {

                string OrgsCongratsJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "OrgsCongrats_SeedData.json");
                var OrgsCongratsJsonTypes = JsonConvert.DeserializeObject<List<OrgsCongrats>>(OrgsCongratsJson);

                if (OrgsCongratsJsonTypes != null && OrgsCongratsJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.OrgsCongrats.Any())
                        {
                            foreach (var LK_IntegrationType in OrgsCongratsJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.OrgsCongrats.AddRange(OrgsCongratsJsonTypes);

                            _dbContext.SaveChanges();
                        }

                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {


            }

        }
        private static void UsersCongrats_SeedData()
        {
            try
            {

                string UsersCongratsJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "UsersCongrats_SeedData.json");
                var UsersCongratsJsonTypes = JsonConvert.DeserializeObject<List<UsersCongrats>>(UsersCongratsJson);

                if (UsersCongratsJsonTypes != null && UsersCongratsJsonTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.UsersCongrats.Any())
                        {
                            foreach (var LK_IntegrationType in UsersCongratsJsonTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.UsersCongrats.AddRange(UsersCongratsJsonTypes);

                            _dbContext.SaveChanges();
                        }

                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {



            }
        }
        private static void TemplateUserException_SeedData()
        {
            try
            {



                string TemplateUserExceptionsJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "TemplateUserException_SeedData.json");
                var TemplateUserExceptionsTypes = JsonConvert.DeserializeObject<List<TemplateUserException>>(TemplateUserExceptionsJson);

                if (TemplateUserExceptionsTypes != null && TemplateUserExceptionsTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        //  _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Categories] ON");
                        if (!_dbContext.TemplateUserException.Any())
                        {
                            foreach (var LK_IntegrationType in TemplateUserExceptionsTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.TemplateUserException.AddRange(TemplateUserExceptionsTypes);

                            _dbContext.SaveChanges();
                        }

                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            }
            catch
            {



            }
        }



        private static void SeedSendTo()
        {
            try
            {
                string SendToJson = File.ReadAllText(@"SeedData" + Path.DirectorySeparatorChar + "SendToDataSeed.json");
                var SendToTypes = JsonConvert.DeserializeObject<List<LK_SendTo>>(SendToJson);

                if (SendToTypes != null && SendToTypes.Any())
                {
                    _dbContext.Database.OpenConnection();
                    try
                    {
                        _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [LK_SendTo] ON");
                        if (!_dbContext.LK_SendTo.Any())
                        {
                            foreach (var LK_IntegrationType in SendToTypes)
                            {
                                LK_IntegrationType.CreatedOn = DateTime.Now;
                            }

                            _dbContext.LK_SendTo.AddRange(SendToTypes);

                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            var existingSendToTypes = _dbContext.LK_SendTo.ToList();
                            var newSendToTypes = SendToTypes
                                .Where(na => !existingSendToTypes.Any(ea => ea.Id == na.Id))
                                .ToList();

                            if (existingSendToTypes.Any())
                            {
                                foreach (var Template in existingSendToTypes)
                                {
                                    Template.CreatedOn = DateTime.Now;
                                }

                                _dbContext.LK_SendTo.AddRange(newSendToTypes);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }


            }
            catch
            {


            }
        }
    }
}
