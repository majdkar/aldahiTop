using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Models.Chat;
using FirstCall.Infrastructure.Models.Identity;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.GeneralSettings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.ExtendedAttributes;
using FirstCall.Domain.Entities.Misc;
using FirstCall.Core.Entities;
using FirstCall.Domain.Entities;
using FirstCall.Domain.Entities.Products;
using static FirstCall.Shared.Constants.Permission.Permissions;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Infrastructure.Contexts
{
    public class BlazorHeroContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public BlazorHeroContext(DbContextOptions<BlazorHeroContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<ChatHistory<BlazorHeroUser>> ChatHistories { get; set; }

        /*s0001s*/
        DbSet<Brand> Brands { get; set; }
        DbSet<Kind> Kinds { get; set; }
        
        DbSet<Princedom> Princedoms { get; set; }
        
        DbSet<Nation> Nations { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<Season> Seasons { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockCategory> BlockCategory { get; set; }
        public DbSet<BlockCategoryTranslation> BlockCategoryTranslation { get; set; }
        public DbSet<BlockTranslation> BlockTranslation { get; set; }
        public DbSet<BlockPhoto> BlockPhotos { get; set; }
        public DbSet<BlockAttachement> BlockAttachements { get; set; }
        public DbSet<BlockVideo> BlockVideos { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductCom> ProductComs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<FirstCall.Domain.Entities.GeneralSettings.Warehouses> Warehousess { get; set; }
        public DbSet<FirstCall.Domain.Entities.Clients.Person>  Persons { get; set; }

      

        public DbSet<Event> Events { get; set; }

        public DbSet<EventCategory> EventCategory { get; set; }
        public DbSet<EventCategoryTranslation> EventCategoryTranslation { get; set; }
        public DbSet<EventTranslation> EventTranslations { get; set; }
        public DbSet<EventPhoto> EventPhotos { get; set; }
        public DbSet<EventAttachement> EventAttachements { get; set; }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuCategoryTranslation> MenuCategoryTranslations { get; set; }
        public DbSet<MenueTranslate> MenueTranslates { get; set; }

        public DbSet<Page> Pages { get; set; }
        public DbSet<PageTranslation> PageTranslations { get; set; }
        public DbSet<PagePhoto> PagePhotos { get; set; }
        public DbSet<PageAttachement> PageAttachements { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<DeliveryOrderProduct> DeliveryOrderProducts { get; set; }

        


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);
            builder.Entity<ChatHistory<BlazorHeroUser>>(entity =>
            {
                entity.ToTable("ChatHistory");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatHistoryFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatHistoryToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<BlazorHeroUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<BlazorHeroRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });

            builder.Entity<BlazorHeroRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });


        
        }
    }
}