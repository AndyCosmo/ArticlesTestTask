using ArticlesTestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ArticlesTestTask.DAL
{
    public class ArticleContext : DbContext
    {
        public ArticleContext(DbContextOptions<ArticleContext> options)
        : base(options)
        { }

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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //
        //optionsBuilder.UseNpgsql();
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tag>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }

        private void AddCreatedUpdatedTimestamps()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries().Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (EntityEntry entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseModel)entry.Entity).CreatedAt = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    ((BaseModel)entry.Entity).UpdatedAt = DateTime.Now;
                }
            }
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Section> Sections { get; set; }

    }
}
