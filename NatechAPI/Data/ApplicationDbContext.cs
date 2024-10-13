using Microsoft.EntityFrameworkCore;
using NatechAPI.Models.Entities;

namespace NatechAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<CatEntity> Cats { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<CatTag> CatTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatEntity>()
                .HasIndex(c => c.CatId).IsUnique();

            modelBuilder.Entity<TagEntity>()
                .HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<CatTag>()
                .HasKey(ct => new { ct.CatEntityId, ct.TagEntityId });

            modelBuilder.Entity<CatTag>()
                .HasOne(ct => ct.CatEntity)
                .WithMany(c => c.CatTags)
                .HasForeignKey(ct => ct.CatEntityId);

            modelBuilder.Entity<CatTag>()
                .HasOne(ct => ct.TagEntity)
                .WithMany(t => t.CatTags)
                .HasForeignKey(ct => ct.TagEntityId);
        }
    }
}

