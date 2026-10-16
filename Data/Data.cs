using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Models;

namespace RaceDayManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserEmail)
                .IsUnique();

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany(u => u.OrganizedEvents)
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade path conflicts for Participants
            modelBuilder.Entity<Participant>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.Track)
                .WithMany(t => t.Participants)
                .HasForeignKey(p => p.TrackID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.Fee)
                .WithMany(f => f.Participants)
                .HasForeignKey(p => p.FeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Keep cascade only for Result since it's a strict 1-to-1 dependency
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Participant)
                .WithOne(p => p.Result)
                .HasForeignKey<Result>(r => r.ParticipantID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}