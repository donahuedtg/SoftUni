namespace DataWork.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using DataWork.Data.Models;

    public class DataWorkDbContext : IdentityDbContext<User>
    {
        public DataWorkDbContext(DbContextOptions<DataWorkDbContext> options)
            : base(options)
        {
        }

        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveDay> LeaveDays { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<ProjectName> ProjectNames { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ProjectName>()
                .HasOne(p => p.User)
                .WithMany(u => u.ProjectNames)
                .HasForeignKey(p => p.UserId);

            builder
                .Entity<Work>()
                .HasOne(w => w.User)
                .WithMany(u => u.Works)
                .HasForeignKey(w => w.UserId);

            builder
                .Entity<Work>()
                .HasOne(w => w.ProjectName)
                .WithMany(p => p.Works)
                .HasForeignKey(w => w.ProjectNameId);

            builder
                .Entity<LeaveDay>()
                .HasOne(ld => ld.User)
                .WithMany(u => u.LeaveDays)
                .HasForeignKey(ld => ld.UserId);

            builder
                .Entity<Leave>()
                .HasOne(l => l.User)
                .WithMany(u => u.Leaves)
                .HasForeignKey(l => l.UserId);


            base.OnModelCreating(builder);
        }
    }
}
