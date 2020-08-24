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
                values: new object[] { new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), true, "fchy", "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E", "fchy", "d9c38590-2718-475b-b080-836b0319944d" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsActive", "Password", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), true, "fzt", "1501CAB6-CA3F-470F-AE5E-1A0B970D1707", "fzt", "317fd686-775d-45e2-bf57-749e1153d9f6" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsActive", "Password", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), true, "bob", "157BBC69-9989-4353-A4B9-02A205678562", "bob", "4b997a64-f851-46d6-b5b7-0a581e61897d" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "Type", "UserId", "Value", "Version" },
                values: new object[,]
                {
                    { new Guid("faf8d364-dd4f-44fb-a9a5-082270fbb40a"), "Name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang Fang", "4ea629a7-a1cd-468f-859f-1fa83b5b5346" },
                    { new Guid("b4a3e4fd-0c8c-4714-89e5-bb75ea975427"), "Email", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "bsmith@gmail.com", "6c1a4bc5-d656-4801-946f-44c7fb9ee6b4" },
                    { new Guid("e397b3fd-03ad-45ea-937c-5ba507f38d6a"), "FamilyName", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Smith", "1b8b1c42-b78e-46ea-abdd-aa2a48989872" },
                    { new Guid("c5c2ca1f-5106-492a-804d-994b4834b6ab"), "GivenName", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob", "6d7e8da0-a6f5-45e5-b919-8022b3f5b25a" },
                    { new Guid("fc0cb468-85ba-48d5-b4ce-d52ea149a5e9"), "Name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob Smith", "94ba3859-5f0e-40ef-8233-a032650291d0" },
                    { new Guid("a7e95bd3-a6a1-4216-9795-1e744d6bda9d"), "Role", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "author", "50caad73-1e4f-4c23-b112-9c5f4189bafc" },
                    { new Guid("67f1b342-368a-4045-a3a8-0866cd561ac1"), "Address", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "29 Spencer Ave.", "079b077c-c1b4-4ead-83e6-d9dd0755934c" },
                    { new Guid("8b44bb38-7da3-422e-b33f-339723a05f1b"), "Email", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "ftt@yahoo.com", "8d4bc42e-6af4-4a40-8e17-a0931a4d800d" },
                    { new Guid("0cb68716-50e4-48c0-9a7c-23edd3de4bb7"), "FamilyName", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Fang", "ad3cb0d7-9105-4faf-87bf-539487558709" },
                    { new Guid("cce9802d-d49a-42a8-950e-c119fe876555"), "GivenName", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao", "008a7eb4-675d-48ea-9550-72fb54c635aa" },
                    { new Guid("517ed90f-0065-4707-b17b-cb5cc717c648"), "Name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao Fang", "670fbcda-7303-4444-8838-8b881ec42e92" },
                    { new Guid("ff31b995-6f62-47c3-becf-ef0b0f0814a2"), "Role", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "author", "a061d990-325f-43e7-875e-602fe07a4218" },
                    { new Guid("3c49c06d-093c-4198-a0b0-6941331aced8"), "Address", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "1750 Bloor St.", "b5194b19-af74-4bbc-a572-55139fc40940" },
                    { new Guid("1dba8fb3-e874-4c22-974e-af4fb7d07a41"), "Email", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "fchy@yahoo.com", "66087583-7033-4bef-8382-4d8c2b670efd" },
                    { new Guid("b2b399d5-31e4-41b8-b6e0-b9a9e8bb6190"), "FamilyName", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Fang", "b5be93f6-6ad0-41d4-bc01-4c3d0b9ac1d8" },
                    { new Guid("e9e275b0-0389-44cb-8310-f69ac513a258"), "GivenName", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang", "80707950-19c1-4b86-a4a5-174ca26418b3" },
                    { new Guid("7d0b8faf-f38d-4f48-8591-c8f640e1be04"), "Address", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "3345 Cardross Rd.", "c35cafe5-3960-4b3a-94b3-24444f441e0a" },
                    { new Guid("79aaf2cd-514a-46f1-b628-e11e034ba921"), "Role", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "guest", "16270b0e-a1e2-4bb0-824d-5e8da231921f" }
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
