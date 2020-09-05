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

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("SecurityCode")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("SecurityCodeExpirationDate")
                        .HasColumnType("datetime2");

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
                            Email = "fchy@yahoo.com",
                            IsActive = true,
                            Password = "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==",
                            SecurityCodeExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Subject = "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E",
                            UserName = "fchy",
                            Version = "5d7d16eb-aed5-46dd-855e-88982b294512"
                        },
                        new
                        {
                            Id = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Email = "ftt@yahoo.com",
                            IsActive = true,
                            Password = "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==",
                            SecurityCodeExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Subject = "1501CAB6-CA3F-470F-AE5E-1A0B970D1707",
                            UserName = "fzt",
                            Version = "9bb85164-452d-49e3-be80-a413148fdb49"
                        },
                        new
                        {
                            Id = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Email = "bsmith@gmail.com",
                            IsActive = true,
                            Password = "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==",
                            SecurityCodeExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Subject = "157BBC69-9989-4353-A4B9-02A205678562",
                            UserName = "bob",
                            Version = "1d46e36b-e71e-4465-b4aa-0441120433af"
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
                            Id = new Guid("7234b25c-fb07-47ff-8581-9bfc67d76489"),
                            Type = "name",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "Chaoyang Fang",
                            Version = "5548c177-c88b-4e26-9fb7-54fb853b2201"
                        },
                        new
                        {
                            Id = new Guid("5371b746-dcae-4a7e-8fa4-17a35408b95a"),
                            Type = "given_name",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "Chaoyang",
                            Version = "b8db009c-c7eb-4549-aa7b-c91cc466311c"
                        },
                        new
                        {
                            Id = new Guid("e124c073-204a-4853-847a-fed4621ba76c"),
                            Type = "family_name",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "Fang",
                            Version = "c77e2640-2375-42a4-83ab-54626ef2ee1a"
                        },
                        new
                        {
                            Id = new Guid("037dd855-f36f-4f5e-99c8-eba2ad8cab49"),
                            Type = "email",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "fchy@yahoo.com",
                            Version = "07b32373-06ab-4bf6-bbd2-c62189fb87e4"
                        },
                        new
                        {
                            Id = new Guid("ba401388-6fa4-424b-a852-c7245ea8fe6a"),
                            Type = "address",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "1750 Bloor St.",
                            Version = "1fbc972d-fac9-4a93-8569-7b1e7f47a6a2"
                        },
                        new
                        {
                            Id = new Guid("94dc95c5-e787-4fd7-8f53-8fdb0a15a114"),
                            Type = "birthdate",
                            UserId = new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"),
                            Value = "1967-03-13",
                            Version = "f251bcf8-f94d-4e4c-8247-f86fdac1cded"
                        },
                        new
                        {
                            Id = new Guid("fa01bf66-ff86-4268-8719-fdeb68359959"),
                            Type = "name",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "Zhitao Fang",
                            Version = "43d293f6-1d53-45e3-a0d4-ad614f0e6629"
                        },
                        new
                        {
                            Id = new Guid("f4e3eb06-a10e-4168-b64c-163ebc73a7ad"),
                            Type = "given_name",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "Zhitao",
                            Version = "924a6c21-9e20-4248-b812-6470c3cfe135"
                        },
                        new
                        {
                            Id = new Guid("481dd248-152c-4b54-8b49-a9a14a26be9c"),
                            Type = "family_name",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "Fang",
                            Version = "816719d2-5151-4d85-84cc-a1ce2143ce6b"
                        },
                        new
                        {
                            Id = new Guid("93363207-8c35-4caa-aaa7-21e602e88644"),
                            Type = "email",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "ftt@yahoo.com",
                            Version = "501687b1-7e62-4357-9df1-c078c442a005"
                        },
                        new
                        {
                            Id = new Guid("5121d2ba-3b9a-4b2b-9279-103f5d196bb0"),
                            Type = "address",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "29 Spencer Ave.",
                            Version = "01db59e1-ec93-4ad3-9a96-8d4c83bda987"
                        },
                        new
                        {
                            Id = new Guid("b7328ceb-3247-46f8-b5d2-b1ca5314f0cb"),
                            Type = "birthdate",
                            UserId = new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"),
                            Value = "1999-09-30",
                            Version = "a567ebd0-dbe3-42e6-9961-748de6d52490"
                        },
                        new
                        {
                            Id = new Guid("db4154eb-ed1d-4736-9436-b8a04a5b2416"),
                            Type = "name",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "Bob Smith",
                            Version = "4521f411-c42e-47cb-880b-c3185242697c"
                        },
                        new
                        {
                            Id = new Guid("41804eea-825d-44d9-a8ff-197399bc3c75"),
                            Type = "given_name",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "Bob",
                            Version = "a6e95523-78f0-4f7d-8c0c-f0794116b947"
                        },
                        new
                        {
                            Id = new Guid("9ec9c181-25e7-4454-87ee-2acc47181568"),
                            Type = "family_name",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "Smith",
                            Version = "9aedabff-282f-4463-aa03-51620904b58c"
                        },
                        new
                        {
                            Id = new Guid("21bb9ef8-21d8-4fdb-a565-144ee2ef48b5"),
                            Type = "email",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "bsmith@gmail.com",
                            Version = "a196e86e-53c8-45e2-b05e-e61cac735815"
                        },
                        new
                        {
                            Id = new Guid("5d351a40-47a2-4f19-a3ce-ceddea869810"),
                            Type = "address",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "3345 Cardross Rd.",
                            Version = "71aa02d6-4e89-43d6-ba75-244cf17c6451"
                        },
                        new
                        {
                            Id = new Guid("5bfe3870-8cbd-4961-8100-7f1bcd28316e"),
                            Type = "birthdate",
                            UserId = new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"),
                            Value = "1987-02-23",
                            Version = "62d8b0ea-4785-4a2c-b624-1d831e0e1daa"
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
