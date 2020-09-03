using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hmm.Dal.Data
{
    public class HmmDataContext : DbContext, IHmmDataContext
    {
        public HmmDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<HmmNote> Notes { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<NoteRender> Renders { get; set; }

        public DbSet<NoteCatalog> Catalogs { get; set; }

        public void Save()
        {
            try
            {
                base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex.Message, ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HmmNote>().ToTable("Notes")
                .Property(n => n.Version)
                .HasColumnName("Ts")
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<NoteCatalog>().ToTable("NoteCatalogs");
            modelBuilder.Entity<NoteRender>().ToTable("NoteRenders");
        }
    }
}