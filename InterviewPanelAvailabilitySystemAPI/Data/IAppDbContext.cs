using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewPanelAvailabilitySystemAPI.Data
{
    public interface IAppDbContext : IDbContext
    {
        public DbSet<Employees> Employee { get; set; }


        public DbSet<JobRole> JobRole { get; set; }

        public DbSet<InterviewSlots> InterviewSlot { get; set; }
        public DbSet<InterviewRounds> InterviewRound { get; set; }

        public DbSet<Timeslot> Timeslot { get; set; }
    }
}
