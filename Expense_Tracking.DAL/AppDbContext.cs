#nullable disable

using Expense_Tracking.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracking.DAL
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users").HasKey(x => x.Id);

                builder.Property(x => x.Count).HasColumnType("decimal");
            });

            modelBuilder.Entity<Expense>(builder =>
            {
                builder.ToTable("Expenses").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.Property(x => x.Sum).HasColumnType("decimal");

                builder.HasOne(x => x.User).WithMany(x => x.Expense).HasForeignKey(x => x.UserId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}