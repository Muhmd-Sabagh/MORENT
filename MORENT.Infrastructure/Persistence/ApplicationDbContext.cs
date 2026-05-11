using Microsoft.EntityFrameworkCore;
using MORENT.Domain.Entities;
using MORENT.Domain.Entities.Dbo;
using MORENT.Domain.Entities.Security;

namespace MORENT.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #region Security Schema
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        #endregion

        #region Dbo Schema
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<CarImage> CarImages => Set<CarImage>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<FavoriteCar> FavoriteCars => Set<FavoriteCar>();
        public DbSet<Rental> Rentals => Set<Rental>();
        public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<CarType> CarTypes => Set<CarType>();
        public DbSet<FuelType> FuelTypes => Set<FuelType>();
        public DbSet<SteeringType> SteeringTypes => Set<SteeringType>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<RentalStatus> RentalStatuses => Set<RentalStatus>();
        #endregion

        // Overriding OnModelCreating to configure relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Security Schema
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Dbo Schema
            // Car Relationships
            modelBuilder.Entity<FavoriteCar>()
                .HasKey(f => new { f.UserId, f.CarId });

            modelBuilder.Entity<FavoriteCar>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteCars)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteCar>()
                .HasOne(f => f.Car)
                .WithMany(c => c.FavoriteCars)
                .HasForeignKey(f => f.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rental Relationships
            modelBuilder.Entity<Rental>()
                .HasOne(b => b.PickUpLocation)
                .WithMany()
                .HasForeignKey(b => b.PickUpLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(b => b.DropOffLocation)
                .WithMany()
                .HasForeignKey(b => b.DropOffLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(b => b.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(b => b.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(b => b.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(b => b.PromoCode)
                .WithMany(p => p.Rentals)
                .HasForeignKey(b => b.PromoCodeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Decimals Precision
            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Car>()
                .Property(c => c.Discount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Car>()
                .Property(c => c.AverageRating)
                .HasColumnType("decimal(3,2)");

            modelBuilder.Entity<Rental>()
                .Property(b => b.Subtotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rental>()
                .Property(b => b.Tax)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rental>()
                .Property(b => b.Discount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rental>()
                .Property(b => b.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PromoCode>()
                .Property(p => p.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<PromoCode>()
                .Property(p => p.DiscountAmount)
                .HasColumnType("decimal(18,2)");
            #endregion
        }

        // Overriding SaveChangesAsync to handle Auditing fields
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
