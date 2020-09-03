﻿// <auto-generated />
using System;
using Hmm.IDP.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hmm.IDP.Migrations.IdentityDb
{
    [DbContext(typeof(IdentityDbContext))]
    partial class IdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hmm.IDP.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Subject")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            IsActive = true,
                            Password = "fchy",
                            Subject = "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E",
                            UserName = "fchy",
                            Version = "08cf6a48-b5d7-4447-aa1e-9b65b4116fa2"
                        },
                        new
                        {
                            Id = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            IsActive = true,
                            Password = "fzt",
                            Subject = "1501CAB6-CA3F-470F-AE5E-1A0B970D1707",
                            UserName = "fzt",
                            Version = "35c44084-6778-4f75-b74a-ba21832b59cc"
                        },
                        new
                        {
                            Id = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            IsActive = true,
                            Password = "bob",
                            Subject = "157BBC69-9989-4353-A4B9-02A205678562",
                            UserName = "bob",
                            Version = "ea4d82fe-1bb4-4f5c-923a-139524b77eb5"
                        });
                });

            modelBuilder.Entity("Hmm.IDP.Entities.UserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8bec2b50-ca9d-4ca5-a7ad-d3349eb0b4ce"),
                            Type = "name",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "Chaoyang Fang",
                            Version = "7cb44d4a-b8f0-4c6e-b2c6-b431d9c84345"
                        },
                        new
                        {
                            Id = new Guid("9f6bf4a9-526c-4c67-983e-e9a85e311651"),
                            Type = "given_name",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "Chaoyang",
                            Version = "16aca772-75cc-476d-85a7-11eaf491e127"
                        },
                        new
                        {
                            Id = new Guid("f8474a01-41a4-4a45-b58a-f8c40938da4d"),
                            Type = "family_name",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "Fang",
                            Version = "e1bf9145-8681-461b-839a-453b60cd63f4"
                        },
                        new
                        {
                            Id = new Guid("4a5eaf01-7efe-4386-90d2-9cfc9dc508b5"),
                            Type = "email",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "fchy@yahoo.com",
                            Version = "99c739f0-e89b-421f-b28d-6445c521cd2a"
                        },
                        new
                        {
                            Id = new Guid("c7884b4f-784e-48a7-b835-a7fd7c54e47e"),
                            Type = "address",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "1750 Bloor St.",
                            Version = "4d1c1880-c170-4abd-9546-4d74fc8e15e6"
                        },
                        new
                        {
                            Id = new Guid("f325a27b-c90b-47bd-b661-936862e5b03e"),
                            Type = "birthdate",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "1967-03-13",
                            Version = "c94c26c6-6eca-4ff4-a8a6-b5121ed3742e"
                        },
                        new
                        {
                            Id = new Guid("4a028bf5-070f-43d6-a8a0-03eb8a958c51"),
                            Type = "name",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "Zhitao Fang",
                            Version = "0ea0e85e-34f7-4a39-89e2-0fd7a479ed82"
                        },
                        new
                        {
                            Id = new Guid("d249498a-a20d-4208-a74c-560589c8e752"),
                            Type = "given_name",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "Zhitao",
                            Version = "6772c277-7e6f-432a-aba1-5c6003097ca3"
                        },
                        new
                        {
                            Id = new Guid("46a6ef68-35f5-42db-8ecb-7e0fa77fa1e1"),
                            Type = "family_name",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "Fang",
                            Version = "3080d2a7-a565-459e-8a1b-be41ae849b60"
                        },
                        new
                        {
                            Id = new Guid("4ae82b81-3113-4a51-b425-4c90d488a781"),
                            Type = "email",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "ftt@yahoo.com",
                            Version = "e56ae42a-0d79-4090-89eb-851d5ec28fe5"
                        },
                        new
                        {
                            Id = new Guid("6135707f-e110-44cc-844e-ec15dee500ff"),
                            Type = "address",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "29 Spencer Ave.",
                            Version = "5a80b3bb-4582-4a8a-aa7a-bf413de45ee5"
                        },
                        new
                        {
                            Id = new Guid("d4e52ec8-8b5b-4ebd-9054-d6bb9097495a"),
                            Type = "birthdate",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "1999-09-30",
                            Version = "088ff9dd-4604-460a-82d9-eac09ceb5c88"
                        },
                        new
                        {
                            Id = new Guid("40707a5f-5bdd-44ef-83fc-0d86e6e237a4"),
                            Type = "name",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "Bob Smith",
                            Version = "5f35faa9-f214-42ab-9e8c-5e94a5f2af75"
                        },
                        new
                        {
                            Id = new Guid("997b0cda-82a7-4f86-b3d0-2a81278fb115"),
                            Type = "given_name",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "Bob",
                            Version = "79706017-7e95-4bb8-952b-75ba5a85a24f"
                        },
                        new
                        {
                            Id = new Guid("3b548e2d-4a30-4080-8b9d-484daa841ba7"),
                            Type = "family_name",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "Smith",
                            Version = "ab6264dc-827b-4735-a6e5-52560f22b3b5"
                        },
                        new
                        {
                            Id = new Guid("af752807-b638-43fa-b461-128f14717514"),
                            Type = "email",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "bsmith@gmail.com",
                            Version = "fca11af4-b470-4047-96fe-ae563247823a"
                        },
                        new
                        {
                            Id = new Guid("81162aac-971a-4963-b34f-b23ce4f08829"),
                            Type = "address",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "3345 Cardross Rd.",
                            Version = "8cf9d7ea-e9a1-4c92-ae1b-6993b759b96f"
                        },
                        new
                        {
                            Id = new Guid("54e8b9f8-518e-4f6c-bd17-3e7d27da0be3"),
                            Type = "birthdate",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "1987-02-23",
                            Version = "e06279fa-6bcc-449f-87d2-7fe660144fae"
                        });
                });

            modelBuilder.Entity("Hmm.IDP.Entities.UserClaim", b =>
                {
                    b.HasOne("Hmm.IDP.Entities.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
