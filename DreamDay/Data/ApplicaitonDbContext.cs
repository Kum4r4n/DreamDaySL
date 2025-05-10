using DreamDay.Entites;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Data;

public class ApplicaitonDbContext : DbContext
{
    public ApplicaitonDbContext(DbContextOptions<ApplicaitonDbContext> options) : base(options)
    {
    }

    public DbSet<Wedding> Weddings { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<BudgetItem> BudgetItems { get; set; }
    public DbSet<CheckListItem> CheckListItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<WeddingNote> WeddingNotes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wedding>()
            .HasMany(w => w.Guests)
            .WithOne(g => g.Wedding)
            .HasForeignKey(g => g.WeddingId);

        modelBuilder.Entity<Wedding>()
            .HasMany(w => w.BudgetItems)
            .WithOne(b => b.Wedding)
            .HasForeignKey(b => b.WeddingId);

        modelBuilder.Entity<Wedding>()
            .HasMany(w => w.CheckListItems)
            .WithOne(b => b.Wedding)
            .HasForeignKey(b => b.WeddingId);

        modelBuilder.Entity<User>()
            .HasMany(w => w.Weddings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);
    }
}