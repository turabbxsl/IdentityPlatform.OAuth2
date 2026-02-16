using IdentityPlatform.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityPlatform.Infrastructure.Persistence.Context
{
    public class PlatformContext : DbContext
    {
        public PlatformContext(DbContextOptions<PlatformContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<OAuthClient> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AuthCode> AuthCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers"); 

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RegistrationNumber).HasColumnName("registrationnumber").IsRequired();
                entity.Property(e => e.FullName).HasColumnName("fullname").IsRequired();
                entity.Property(e => e.IdentityNumber).HasColumnName("identitynumber").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
                entity.Property(e => e.IsActive).HasColumnName("isactive").HasDefaultValue(true);

                entity.Property(e => e.CreatedAt).HasColumnName("createdat").HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(e => e.RegistrationNumber).IsUnique();
                entity.HasIndex(e => e.IdentityNumber).IsUnique();

                entity.HasMany(e => e.RefreshTokens)
                      .WithOne(rt => rt.Customer)
                      .HasForeignKey(rt => rt.CustomerId);
            });

            modelBuilder.Entity<OAuthClient>(entity =>
            {
                entity.ToTable("oauthclients");

                entity.HasKey(e => e.ClientId);

                entity.Property(e => e.ClientId).HasColumnName("clientid");
                entity.Property(e => e.ClientSecret).HasColumnName("clientsecret").IsRequired();
                entity.Property(e => e.ClientName).HasColumnName("clientname").IsRequired();
                entity.Property(e => e.RedirectUri).HasColumnName("redirecturi");
                entity.Property(e => e.AllowedScopes).HasColumnName("allowedscopes");

                entity.Property(e => e.CreatedAt).HasColumnName("createdat").HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refreshtokens");

                entity.HasKey(e => e.Token);

                entity.Property(e => e.Token).HasColumnName("token");
                entity.Property(e => e.CustomerId).HasColumnName("customerid");
                entity.Property(e => e.ClientId).HasColumnName("clientid");
                entity.Property(e => e.ExpiresAt).HasColumnName("expiresat");
                entity.Property(e => e.IsRevoked).HasColumnName("isrevoked");

                entity.Property(e => e.CreatedAt).HasColumnName("createdat").HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<AuthCode>(entity =>
            {
                entity.ToTable("authcodes");

                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.CustomerId).HasColumnName("customerid");
                entity.Property(e => e.ClientId).HasColumnName("clientid");
                entity.Property(e => e.Scope).HasColumnName("scope");
                entity.Property(e => e.ExpiresAt).HasColumnName("expiresat");
                entity.Property(e => e.IsUsed).HasColumnName("isused").HasDefaultValue(false);

                entity.Property(e => e.CreatedAt).HasColumnName("createdAt").HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.Customer)
                      .WithMany()
                      .HasForeignKey(e => e.CustomerId);

                entity.HasOne(e => e.Client)
                      .WithMany()
                      .HasForeignKey(e => e.ClientId);
            });
        }

    }
}
