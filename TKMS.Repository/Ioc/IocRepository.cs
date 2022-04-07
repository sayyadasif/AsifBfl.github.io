using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Repository.Contexts;
using TKMS.Repository.Interfaces;
using TKMS.Repository.Repositories;

namespace TKMS.Repository.Ioc
{
    public static class IocRepository
    {
        public static void RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<TkmsDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }),
            ServiceLifetime.Scoped//Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );

            #region Repositories

            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IBranchDispatchRepository, BranchDispatchRepository>();
            services.AddTransient<IBranchTransferRepository, BranchTransferRepository>();
            services.AddTransient<IBranchTypeRepository, BranchTypeRepository>();
            services.AddTransient<IC5CodeRepository, C5CodeRepository>();
            services.AddTransient<ICardTypeRepository, CardTypeRepository>();
            services.AddTransient<ICourierStatusRepository, CourierStatusRepository>();
            services.AddTransient<IDispatchAuditRepository, DispatchAuditRepository>();
            services.AddTransient<IDispatchRepository, DispatchRepository>();
            services.AddTransient<IDispatchStatusRepository, DispatchStatusRepository>();
            services.AddTransient<IDispatchWayBillRepository, DispatchWayBillRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IIblBranchRepository, IblBranchRepository>();
            services.AddTransient<IIndentAuditRepository, IndentAuditRepository>();
            services.AddTransient<IIndentRepository, IndentRepository>();
            services.AddTransient<IIndentStatusRepository, IndentStatusRepository>();
            services.AddTransient<IKitRepository, KitRepository>();
            services.AddTransient<IKitAssignedCustomerRepository, KitAssignedCustomerRepository>();
            services.AddTransient<IKitAuditRepository, KitAuditRepository>();
            services.AddTransient<IKitStatusRepository, KitStatusRepository>();
            services.AddTransient<IKitDamageReasonRepository, KitDamageReasonRepository>();
            services.AddTransient<IKitReturnRepository, KitReturnRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<INotificationTypeRepository, NotificationTypeRepository>();
            services.AddTransient<IRegionRepository, RegionRepository>();
            services.AddTransient<IRejectionReasonRepository, RejectionReasonRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleTypeRepository, RoleTypeRepository>();
            services.AddTransient<ISchemeCodeRepository, SchemeCodeRepository>();
            services.AddTransient<ISentSmsRepository, SentSmsRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IIWorksKitRepository, IWorksKitRepository>();

            #endregion
        }
    }
}
