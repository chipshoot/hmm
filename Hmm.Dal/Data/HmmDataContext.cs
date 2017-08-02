using DomainEntity.Misc;
using DomainEntity.User;
using Microsoft.EntityFrameworkCore;

namespace Hmm.Dal.Data
{
    public class HmmDataContext : DbContext
    {
        public HmmDataContext(DbContextOptions<HmmDataContext> options) : base(options)
        {
        }

        public DbSet<HmmNote> Notes { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<NoteRender> Renders { get; set; }

        public DbSet<NoteCatalog> Catalogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HmmNote>().ToTable("Notes")
                .Property(n => n.Version)
                .HasColumnName("Ts")
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<NoteCatalog>().ToTable("NoteCatalogues");
            modelBuilder.Entity<NoteRender>().ToTable("NoteRenders");
        }
    }
}