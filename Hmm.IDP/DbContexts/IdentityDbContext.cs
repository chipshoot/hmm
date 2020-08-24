using Hmm.IDP.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hmm.Contract;

namespace Hmm.IDP.DbContexts
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Subject)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Subject = "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E",
                    UserName = "fchy",
                    Password = "fchy",
                    IsActive = true
                },
                new User
                {
                    Id = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Subject = "1501CAB6-CA3F-470F-AE5E-1A0B970D1707",
                    UserName = "fzt",
                    Password = "fzt",
                    IsActive = true
                },
                new User
                {
                    Id = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Subject = "157BBC69-9989-4353-A4B9-02A205678562",
                    UserName = "bob",
                    Password = "bob",
                    IsActive = true
                }
            );

            modelBuilder.Entity<UserClaim>().HasData(

                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = "name",
                    Value = "Chaoyang Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = "givenname",
                    Value = "Chaoyang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = "familyname",
                    Value = "Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = "email",
                    Value = "fchy@yahoo.com"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = "address",
                    Value = "1750 Bloor St."
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = "role",
                    Value = HmmConstants.Roles.Author
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = "name",
                    Value = "Zhitao Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = "givenname",
                    Value = "Zhitao"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = "familyname",
                    Value = "Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = "email",
                    Value = "ftt@yahoo.com"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = "address",
                    Value = "29 Spencer Ave."
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = "role",
                    Value = HmmConstants.Roles.Author
                },


                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = "name",
                    Value = "Bob Smith"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = "givenname",
                    Value = "Bob"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = "familyname",
                    Value = "Smith"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = "email",
                    Value = "bsmith@gmail.com"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = "address",
                    Value = "3345 Cardross Rd."
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = "role",
                    Value = HmmConstants.Roles.Guest
                }
            );
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // get update entries
            var updatedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .OfType<VersionAwareEntity>();

            foreach (var entry in updatedEntries)
            {
                entry.Version = Guid.NewGuid().ToString();
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}