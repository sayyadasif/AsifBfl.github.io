using Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TKMS.Repository.Ioc;
using TKMS.Service.Interfaces;
using TKMS.Service.Services;

namespace TKMS.Service.Ioc
{
    public static class IocService
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterRepositories(configuration);

            #region Services

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITokenBuilder, TokenBuilder>();
            services.AddTransient<IUserProviderService, UserProviderService>();

            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IBranchService, BranchService>();
            services.AddTransient<IBranchDispatchService, BranchDispatchService>();
            services.AddTransient<IBranchTransferService, BranchTransferService>();
            services.AddTransient<IBranchTypeService, BranchTypeService>();
            services.AddTransient<IC5CodeService, C5CodeService>();
            services.AddTransient<ICardTypeService, CardTypeService>();
            services.AddTransient<ICourierStatusService, CourierStatusService>();
            services.AddTransient<IDispatchAuditService, DispatchAuditService>();
            services.AddTransient<IDispatchService, DispatchService>();
            services.AddTransient<IDispatchStatusService, DispatchStatusService>();
            services.AddTransient<IDispatchWayBillService, DispatchWayBillService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IIblBranchService, IblBranchService>();
            services.AddTransient<IIndentAuditService, IndentAuditService>();
            services.AddTransient<IIndentService, IndentService>();
            services.AddTransient<IIndentStatusService, IndentStatusService>();
            services.AddTransient<IKitService, KitService>();
            services.AddTransient<IKitAuditService, KitAuditService>();
            services.AddTransient<IKitDamageReasonService, KitDamageReasonService>();
            services.AddTransient<IKitStatusService, KitStatusService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationTypeService, NotificationTypeService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<IRejectionReasonService, RejectionReasonService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IRoleTypeService, RoleTypeService>();
            services.AddTransient<ISchemeCodeService, SchemeCodeService>();
            services.AddTransient<ISentSmsService, SentSmsService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IIWorksKitService, IWorksKitService>();

            #endregion
        }
    }
}
