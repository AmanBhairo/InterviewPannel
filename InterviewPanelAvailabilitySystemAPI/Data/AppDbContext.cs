using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace InterviewPanelAvailabilitySystemAPI.Data
{
    public class AppDbContext : DbContext,IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employees> Employee { get; set; }


        public DbSet<JobRole> JobRole { get; set; }

        public DbSet<InterviewSlots> InterviewSlot { get; set; }
        public DbSet<InterviewRounds> InterviewRound { get; set; }

        public DbSet<Timeslot> Timeslot { get; set; }

        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }

        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }

    }
}
