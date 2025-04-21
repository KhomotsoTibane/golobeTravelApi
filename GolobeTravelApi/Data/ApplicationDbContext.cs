using GolobeTravelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GolobeTravelApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HotelData> TblHotelData { get; set; }
        public DbSet<HotelEntity> TblHotelEntities { get; set; }
        public DbSet<User> TblUser { get; set; }
        public DbSet<UserFavorites> TblUserFavorites { get; set; }
        public DbSet<UserBookings> TblUserBookings { get; set; }

        public DbSet<HotelBooking> TblHotelBookings { get; set; }

        public DbSet<HotelReviews> TblHotelReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // HotelData
            modelBuilder.Entity<HotelData>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.HasOne(h => h.Entity)
                      .WithMany(e => e.Hotels)
                      .HasForeignKey(h => h.EntityId)
                      .IsRequired();
            });

            // HotelEntity
            modelBuilder.Entity<HotelEntity>(entity =>
            {
                entity.HasKey(e => e.EntityId);
            });

            modelBuilder.Entity<HotelReviews>()
                .HasKey(r => r.ReviewId);



            // Hotel Reviews
            modelBuilder.Entity<HotelReviews>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(r => r.HotelId);

            // Hotel Bookings
            modelBuilder.Entity<HotelBooking>(entity =>
            {
                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.Status).HasDefaultValue("Pending");
                entity.Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .IsRequired();

                entity.HasOne(b => b.Hotel)
                    .WithMany()
                    .HasForeignKey(b => b.HotelId)
                    .HasPrincipalKey(h => h.HotelId);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
