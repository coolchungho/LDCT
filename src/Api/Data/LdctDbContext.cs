using LDCT.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LDCT.Api.Data;

public class LdctDbContext(DbContextOptions<LdctDbContext> options) : DbContext(options)
{
    public DbSet<LdctCase> Cases => Set<LdctCase>();
    public DbSet<FollowUpLogEntry> FollowUpLogs => Set<FollowUpLogEntry>();
    public DbSet<SmsMessage> SmsMessages => Set<SmsMessage>();
    public DbSet<OrphanLdctReportRow> OrphanLdctReports => Set<OrphanLdctReportRow>();
    public DbSet<IntegrationSettingRecord> IntegrationSettings => Set<IntegrationSettingRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LdctCase>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.MedicalRecordNumber).IsUnique();
            e.Property(x => x.MedicalRecordNumber).HasMaxLength(64);
            e.Property(x => x.PatientName).HasMaxLength(128);
            e.Property(x => x.MaxNoduleLengthMm).HasPrecision(10, 2);
        });

        modelBuilder.Entity<FollowUpLogEntry>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Case).WithMany(c => c.FollowUpLogs).HasForeignKey(x => x.CaseId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.CaseId, x.ContactDate });
        });

        modelBuilder.Entity<SmsMessage>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Case).WithMany(c => c.SmsMessages).HasForeignKey(x => x.CaseId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrphanLdctReportRow>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.ReceivedAt);
        });

        modelBuilder.Entity<IntegrationSettingRecord>(e =>
        {
            e.HasKey(x => x.Key);
            e.Property(x => x.Key).HasMaxLength(128);
        });
    }
}
