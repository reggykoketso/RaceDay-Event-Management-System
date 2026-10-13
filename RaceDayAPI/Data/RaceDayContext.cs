using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Models;

namespace RaceDayAPI.Data
{
    public class RaceDayContext : DbContext
    {
        public RaceDayContext(DbContextOptions<RaceDayContext> options)
            : base(options)
        { }
      

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<RaceDayAPI.Models.Route> Routes { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<User>()
               .Property(u => u.UserID)
               .HasColumnName("UserID");

            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .HasColumnName("FirstName");

            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .HasColumnName("LastName");

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnName("Email");

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnName("PasswordHash");

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasColumnName("Role");

            modelBuilder.Entity<User>()
                .Property(u => u.Phone)
                .HasColumnName("Phone");

            modelBuilder.Entity<Event>()
                .HasKey(e => e.EventID);

            modelBuilder.Entity<Event>()
               .Property(e => e.EventID)
               .HasColumnName("EventID");

            modelBuilder.Entity<Event>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.OrganiserId);

            modelBuilder.Entity<Event>()
                .Property(e => e.OrganiserId)
                .HasColumnName("OrganiserID");

            modelBuilder.Entity<Event>()
                .Property(e => e.Description)
                .HasColumnName("Description");

            modelBuilder.Entity<Event>()
                .Property(e => e.EventDate)
                .HasColumnName("EventDate");

            modelBuilder.Entity<Event>()
                .Property(e => e.Distance)
                .HasColumnName("Distance")
                .HasPrecision(6, 2);

            modelBuilder.Entity<Event>()
                .Property(e => e.EventType)
                .HasColumnName("EventType");
           


        }
    }
}
