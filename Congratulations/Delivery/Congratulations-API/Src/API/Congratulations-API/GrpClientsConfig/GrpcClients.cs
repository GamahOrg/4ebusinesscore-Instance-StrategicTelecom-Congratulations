



using Aggregator.GrpClientsConfig;

namespace _4EPlatform_StrategicTelecom_Medals_Api.GrpClientsConfig
{

    public static class GrpcClients
    {
        public static void AddUserManagementGrpcClients(
  this IServiceCollection services,
  string UserManagmentUrl)
        {
            services.AddGrpcServiceClient<PermissionGroupingService.PermissionGroupingServiceClient>(UserManagmentUrl);
            //services.AddGrpcServiceClient<UserProtoService.UserProtoServiceClient>(UserManagmentUrl);
        }

        #region controlPanelAggregator
        public static void AddControlPanelAggGrpcClients(this IServiceCollection services, string url)
        {
            //services.AddGrpcServiceClient<UserOrgProtoService.UserOrgProtoServiceClient>(url);
            //services.AddGrpcServiceClient<DelegationUserProtoService.DelegationUserProtoServiceClient>(url);
            //services.AddGrpcServiceClient<UserProtoService.UserProtoServiceClient>(url);
            //services.AddGrpcServiceClient<RoleWorkspaceProtoService.RoleWorkspaceProtoServiceClient>(url);
            ////services.AddGrpcServiceClient<OrgMembersProtoService.OrgMembersProtoServiceClient>(url);
            //services.AddGrpcServiceClient<UserDelegationProtoService.UserDelegationProtoServiceClient>(url);
            //services.AddGrpcServiceClient<UserDelegationProtoService.UserDelegationProtoServiceClient>(url);
        }
        #endregion

        #region controlPanelAggregator
        public static void AddStrategicTelecomAggGrpcClients(this IServiceCollection services, string url)
        {
           // services.AddGrpcServiceClient<UserProtoService.UserProtoServiceClient>(url);
            //services.AddGrpcServiceClient<DelegationUserProtoService.DelegationUserProtoServiceClient>(url);
            //services.AddGrpcServiceClient<UserProtoService.UserProtoServiceClient>(url);
            //services.AddGrpcServiceClient<RoleWorkspaceProtoService.RoleWorkspaceProtoServiceClient>(url);
          //  services.AddGrpcServiceClient<OrgProtoService.OrgProtoServiceClient>(url);
            //services.AddGrpcServiceClient<UserDelegationProtoService.UserDelegationProtoServiceClient>(url);
            //services.AddGrpcServiceClient<UserDelegationProtoService.UserDelegationProtoServiceClient>(url);

        }
        #endregion

    }
}
