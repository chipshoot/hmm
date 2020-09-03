using Hmm.IDP.Entities;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                    Password = "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==",
                    IsActive = true
                },
                new User
                {
                    Id = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Subject = "1501CAB6-CA3F-470F-AE5E-1A0B970D1707",
                    UserName = "fzt",
                    Password = "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==",
                    IsActive = true
                },
                new User
                {
                    Id = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Subject = "157BBC69-9989-4353-A4B9-02A205678562",
                    UserName = "bob",
                    Password = "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==",
                    IsActive = true
                }
            );

            modelBuilder.Entity<UserClaim>().HasData(

                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = JwtClaimTypes.Name,
                    Value = "Chaoyang Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = JwtClaimTypes.GivenName,
                    Value = "Chaoyang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = JwtClaimTypes.FamilyName,
                    Value = "Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = JwtClaimTypes.Email,
                    Value = "fchy@yahoo.com"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = JwtClaimTypes.Address,
                    Value = "1750 Bloor St."
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("08E54EF4-F64C-442B-BD4D-63DF65BDFB55"),
                    Type = JwtClaimTypes.BirthDate,
                    Value = "1967-03-13"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = JwtClaimTypes.Name,
                    Value = "Zhitao Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = JwtClaimTypes.GivenName,
                    Value = "Zhitao"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = JwtClaimTypes.FamilyName,
                    Value = "Fang"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = JwtClaimTypes.Email,
                    Value = "ftt@yahoo.com"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = JwtClaimTypes.Address,
                    Value = "29 Spencer Ave."
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("303CAC10-A6C6-4B46-846B-AA07A8D46393"),
                    Type = JwtClaimTypes.BirthDate,
                    Value = "1999-09-30"
                },

                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = JwtClaimTypes.Name,
                    Value = "Bob Smith"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = JwtClaimTypes.GivenName,
                    Value = "Bob"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = JwtClaimTypes.FamilyName,
                    Value = "Smith"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = JwtClaimTypes.Email,
                    Value = "bsmith@gmail.com"
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = JwtClaimTypes.Address,
                    Value = "3345 Cardross Rd."
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("3DD54A32-688F-40FB-9F5E-666FF007B3C1"),
                    Type = JwtClaimTypes.BirthDate,
                    Value = "1987-02-23"
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