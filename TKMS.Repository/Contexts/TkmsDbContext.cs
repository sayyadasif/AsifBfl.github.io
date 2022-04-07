using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Contexts
{
    public class TkmsDbContext : DbContext
    {
        public TkmsDbContext(DbContextOptions<TkmsDbContext> options)
             : base(options)
        { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchDispatch> BranchDispatches { get; set; }
        public DbSet<BranchTransfer> BranchTransfers { get; set; }
        public DbSet<BranchType> BranchTypes { get; set; }
        public DbSet<C5Code> C5Codes { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<CourierStatus> CourierStatuses { get; set; }
        public DbSet<DispatchAudit> DispatchAudits { get; set; }
        public DbSet<Dispatch> Dispatches { get; set; }
        public DbSet<DispatchStatus> DispatchStatuses { get; set; }
        public DbSet<DispatchWayBill> DispatchWayBills { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<IblBranch> IblBranches { get; set; }
        public DbSet<IndentAudit> IndentAudits { get; set; }
        public DbSet<Indent> Indents { get; set; }
        public DbSet<IndentStatus> IndentStatuses { get; set; }
        public DbSet<Kit> Kits { get; set; }
        public DbSet<KitAudit> KitAudits { get; set; }
        public DbSet<KitAssignedCustomer> KitAssignedCustomers { get; set; }
        public DbSet<KitReturn> KitReturns { get; set; }
        public DbSet<KitDamageReason> KitDamageReasons { get; set; }
        public DbSet<KitStatus> KitStatuses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RejectionReason> RejectionReasons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<SchemeCode> SchemeCodes { get; set; }
        public DbSet<SchemeC5Code> SchemeC5Codes { get; set; }
        public DbSet<SentSms> SentSmses { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<IWorksKit> IWorksKits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
