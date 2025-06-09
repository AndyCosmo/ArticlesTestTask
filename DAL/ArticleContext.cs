using ArticlesTestTask.DAL.Models;
using ArticlesTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Emit;

namespace ArticlesTestTask.DAL
{
    public class ArticleContext : DbContext
    {
        private readonly IDateTimeService _dateTimeService;

        public ArticleContext(DbContextOptions<ArticleContext> options,
            IDateTimeService dateTimeService)
        : base(options)
        {
            _dateTimeService = dateTimeService;
        }

        public override int SaveChanges()
        {
            AddCreatedUpdatedTimestamps();
            return base.SaveChanges();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddCreatedUpdatedTimestamps();
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tag>()
                .HasIndex(u => u.NameLower)
                .IsUnique();

            builder.Entity<Article>()
                .Property(a => a.LastActivityAt)
                .HasComputedColumnSql("COALESCE(updated_at, created_at)", true);

            builder.Entity<Article>()
                .HasIndex(a => new { a.SectionId, a.LastActivityAt });

            builder.Entity<ArticleTag>()
                .HasKey(at => new { at.ArticleId, at.TagId });
        }

        private void AddCreatedUpdatedTimestamps()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries().Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (EntityEntry entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseModel)entry.Entity).CreatedAt = _dateTimeService.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    ((BaseModel)entry.Entity).UpdatedAt = _dateTimeService.UtcNow;
                }
            }
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Section> Sections { get; set; }

    }
}
