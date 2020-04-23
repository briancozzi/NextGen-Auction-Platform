using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Authorization.Delegation;
using NextGen.BiddingPlatform.Authorization.Roles;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Chat;
using NextGen.BiddingPlatform.Core.Addresses;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using NextGen.BiddingPlatform.Core.AppAccounts;
using NextGen.BiddingPlatform.Core.AuctionBidders;
using NextGen.BiddingPlatform.Core.AuctionHistories;
using NextGen.BiddingPlatform.Core.AuctionItems;
using NextGen.BiddingPlatform.Core.Auctions;
using NextGen.BiddingPlatform.Core.CardDetails;
using NextGen.BiddingPlatform.Core.Categories;
using NextGen.BiddingPlatform.Core.Invoices;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.Core.PaymentTransactions;
using NextGen.BiddingPlatform.Core.State;
using NextGen.BiddingPlatform.Editions;
using NextGen.BiddingPlatform.Friendships;
using NextGen.BiddingPlatform.MultiTenancy;
using NextGen.BiddingPlatform.MultiTenancy.Accounting;
using NextGen.BiddingPlatform.MultiTenancy.Payments;
using NextGen.BiddingPlatform.Storage;
using Toolbelt.ComponentModel.DataAnnotations;

namespace NextGen.BiddingPlatform.EntityFrameworkCore
{
    public class BiddingPlatformDbContext : AbpZeroDbContext<Tenant, Role, User, BiddingPlatformDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<MultiTenancy.Accounting.Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }
        public  virtual DbSet<Country.Country> Countries { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<AppAccount> AppAccounts { get; set; }
        public virtual DbSet<Event> AppAccountEvents { get; set; }
        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<AuctionItem> AuctionItems { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemGallery> ItemGalleries { get; set; }
        public virtual DbSet<AuctionBidder> AuctionBidders { get; set; }
        public virtual DbSet<AuctionHistory> AuctionHistories { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Core.Invoices.Invoice> UserInvoices { get; set; }
        public virtual DbSet<CardDetail> CardDetails { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public BiddingPlatformDbContext(DbContextOptions<BiddingPlatformDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            // Remove when https://github.com/aspnetboilerplate/aspnetboilerplate/issues/5457 is fixed
            modelBuilder.Entity<OrganizationUnit>().HasIndex(e => new { e.TenantId, e.Code }).IsUnique(false);

            //make columns unique and non-cluster using fluent api
            //modelBuilder.Entity<Country>(b =>
            //{
            //    b.HasIndex(x => x.UniqueId)
            //    .IsClustered(false)
            //    .HasName("IX_Country")
            //    .IsUnique();
            //});

            //modelBuilder.Entity<State>(b =>
            //{
            //    b.HasIndex(x => x.UniqueId)
            //    .IsClustered(false)
            //    .HasName("IX_State")
            //    .IsUnique();
            //});
            modelBuilder.ConfigurePersistedGrantEntity();
            modelBuilder.BuildIndexesFromAnnotations();
        }
    }
}
