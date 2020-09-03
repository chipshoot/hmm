using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hmm.IDP.Migrations.IdentityDb
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(maxLength: 200, nullable: false),
                    UserName = table.Column<string>(maxLength: 200, nullable: true),
                    Password = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsActive", "Password", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), true, "fchy", "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E", "fchy", "08cf6a48-b5d7-4447-aa1e-9b65b4116fa2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsActive", "Password", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), true, "fzt", "1501CAB6-CA3F-470F-AE5E-1A0B970D1707", "fzt", "35c44084-6778-4f75-b74a-ba21832b59cc" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsActive", "Password", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), true, "bob", "157BBC69-9989-4353-A4B9-02A205678562", "bob", "ea4d82fe-1bb4-4f5c-923a-139524b77eb5" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "Type", "UserId", "Value", "Version" },
                values: new object[,]
                {
                    { new Guid("8bec2b50-ca9d-4ca5-a7ad-d3349eb0b4ce"), "name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang Fang", "7cb44d4a-b8f0-4c6e-b2c6-b431d9c84345" },
                    { new Guid("af752807-b638-43fa-b461-128f14717514"), "email", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "bsmith@gmail.com", "fca11af4-b470-4047-96fe-ae563247823a" },
                    { new Guid("3b548e2d-4a30-4080-8b9d-484daa841ba7"), "family_name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Smith", "ab6264dc-827b-4735-a6e5-52560f22b3b5" },
                    { new Guid("997b0cda-82a7-4f86-b3d0-2a81278fb115"), "given_name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob", "79706017-7e95-4bb8-952b-75ba5a85a24f" },
                    { new Guid("40707a5f-5bdd-44ef-83fc-0d86e6e237a4"), "name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob Smith", "5f35faa9-f214-42ab-9e8c-5e94a5f2af75" },
                    { new Guid("d4e52ec8-8b5b-4ebd-9054-d6bb9097495a"), "birthdate", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "1999-09-30", "088ff9dd-4604-460a-82d9-eac09ceb5c88" },
                    { new Guid("6135707f-e110-44cc-844e-ec15dee500ff"), "address", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "29 Spencer Ave.", "5a80b3bb-4582-4a8a-aa7a-bf413de45ee5" },
                    { new Guid("4ae82b81-3113-4a51-b425-4c90d488a781"), "email", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "ftt@yahoo.com", "e56ae42a-0d79-4090-89eb-851d5ec28fe5" },
                    { new Guid("46a6ef68-35f5-42db-8ecb-7e0fa77fa1e1"), "family_name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Fang", "3080d2a7-a565-459e-8a1b-be41ae849b60" },
                    { new Guid("d249498a-a20d-4208-a74c-560589c8e752"), "given_name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao", "6772c277-7e6f-432a-aba1-5c6003097ca3" },
                    { new Guid("4a028bf5-070f-43d6-a8a0-03eb8a958c51"), "name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao Fang", "0ea0e85e-34f7-4a39-89e2-0fd7a479ed82" },
                    { new Guid("f325a27b-c90b-47bd-b661-936862e5b03e"), "birthdate", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "1967-03-13", "c94c26c6-6eca-4ff4-a8a6-b5121ed3742e" },
                    { new Guid("c7884b4f-784e-48a7-b835-a7fd7c54e47e"), "address", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "1750 Bloor St.", "4d1c1880-c170-4abd-9546-4d74fc8e15e6" },
                    { new Guid("4a5eaf01-7efe-4386-90d2-9cfc9dc508b5"), "email", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "fchy@yahoo.com", "99c739f0-e89b-421f-b28d-6445c521cd2a" },
                    { new Guid("f8474a01-41a4-4a45-b58a-f8c40938da4d"), "family_name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Fang", "e1bf9145-8681-461b-839a-453b60cd63f4" },
                    { new Guid("9f6bf4a9-526c-4c67-983e-e9a85e311651"), "given_name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang", "16aca772-75cc-476d-85a7-11eaf491e127" },
                    { new Guid("81162aac-971a-4963-b34f-b23ce4f08829"), "address", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "3345 Cardross Rd.", "8cf9d7ea-e9a1-4c92-ae1b-6993b759b96f" },
                    { new Guid("54e8b9f8-518e-4f6c-bd17-3e7d27da0be3"), "birthdate", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "1987-02-23", "e06279fa-6bcc-449f-87d2-7fe660144fae" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Subject",
                table: "Users",
                column: "Subject",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
