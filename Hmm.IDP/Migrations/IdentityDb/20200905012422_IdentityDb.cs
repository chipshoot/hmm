using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hmm.IDP.Migrations.IdentityDb
{
    public partial class IdentityDb : Migration
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
                    Email = table.Column<string>(maxLength: 500, nullable: true),
                    SecurityCode = table.Column<string>(maxLength: 200, nullable: true),
                    SecurityCodeExpirationDate = table.Column<DateTime>(nullable: false),
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
                columns: new[] { "Id", "Email", "IsActive", "Password", "SecurityCode", "SecurityCodeExpirationDate", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "fchy@yahoo.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E", "fchy", "5d7d16eb-aed5-46dd-855e-88982b294512" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Password", "SecurityCode", "SecurityCodeExpirationDate", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "ftt@yahoo.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1501CAB6-CA3F-470F-AE5E-1A0B970D1707", "fzt", "9bb85164-452d-49e3-be80-a413148fdb49" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Password", "SecurityCode", "SecurityCodeExpirationDate", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "bsmith@gmail.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "157BBC69-9989-4353-A4B9-02A205678562", "bob", "1d46e36b-e71e-4465-b4aa-0441120433af" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "Type", "UserId", "Value", "Version" },
                values: new object[,]
                {
                    { new Guid("7234b25c-fb07-47ff-8581-9bfc67d76489"), "name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang Fang", "5548c177-c88b-4e26-9fb7-54fb853b2201" },
                    { new Guid("21bb9ef8-21d8-4fdb-a565-144ee2ef48b5"), "email", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "bsmith@gmail.com", "a196e86e-53c8-45e2-b05e-e61cac735815" },
                    { new Guid("9ec9c181-25e7-4454-87ee-2acc47181568"), "family_name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Smith", "9aedabff-282f-4463-aa03-51620904b58c" },
                    { new Guid("41804eea-825d-44d9-a8ff-197399bc3c75"), "given_name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob", "a6e95523-78f0-4f7d-8c0c-f0794116b947" },
                    { new Guid("db4154eb-ed1d-4736-9436-b8a04a5b2416"), "name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob Smith", "4521f411-c42e-47cb-880b-c3185242697c" },
                    { new Guid("b7328ceb-3247-46f8-b5d2-b1ca5314f0cb"), "birthdate", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "1999-09-30", "a567ebd0-dbe3-42e6-9961-748de6d52490" },
                    { new Guid("5121d2ba-3b9a-4b2b-9279-103f5d196bb0"), "address", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "29 Spencer Ave.", "01db59e1-ec93-4ad3-9a96-8d4c83bda987" },
                    { new Guid("93363207-8c35-4caa-aaa7-21e602e88644"), "email", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "ftt@yahoo.com", "501687b1-7e62-4357-9df1-c078c442a005" },
                    { new Guid("481dd248-152c-4b54-8b49-a9a14a26be9c"), "family_name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Fang", "816719d2-5151-4d85-84cc-a1ce2143ce6b" },
                    { new Guid("f4e3eb06-a10e-4168-b64c-163ebc73a7ad"), "given_name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao", "924a6c21-9e20-4248-b812-6470c3cfe135" },
                    { new Guid("fa01bf66-ff86-4268-8719-fdeb68359959"), "name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao Fang", "43d293f6-1d53-45e3-a0d4-ad614f0e6629" },
                    { new Guid("94dc95c5-e787-4fd7-8f53-8fdb0a15a114"), "birthdate", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "1967-03-13", "f251bcf8-f94d-4e4c-8247-f86fdac1cded" },
                    { new Guid("ba401388-6fa4-424b-a852-c7245ea8fe6a"), "address", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "1750 Bloor St.", "1fbc972d-fac9-4a93-8569-7b1e7f47a6a2" },
                    { new Guid("037dd855-f36f-4f5e-99c8-eba2ad8cab49"), "email", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "fchy@yahoo.com", "07b32373-06ab-4bf6-bbd2-c62189fb87e4" },
                    { new Guid("e124c073-204a-4853-847a-fed4621ba76c"), "family_name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Fang", "c77e2640-2375-42a4-83ab-54626ef2ee1a" },
                    { new Guid("5371b746-dcae-4a7e-8fa4-17a35408b95a"), "given_name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang", "b8db009c-c7eb-4549-aa7b-c91cc466311c" },
                    { new Guid("5d351a40-47a2-4f19-a3ce-ceddea869810"), "address", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "3345 Cardross Rd.", "71aa02d6-4e89-43d6-ba75-244cf17c6451" },
                    { new Guid("5bfe3870-8cbd-4961-8100-7f1bcd28316e"), "birthdate", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "1987-02-23", "62d8b0ea-4785-4a2c-b624-1d831e0e1daa" }
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
