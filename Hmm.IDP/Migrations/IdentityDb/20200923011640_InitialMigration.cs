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
                    Version = table.Column<string>(maxLength: 36, nullable: true),
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
                    Version = table.Column<string>(maxLength: 36, nullable: true),
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
                values: new object[] { new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "fchy@yahoo.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E", "fchy", "1840a20b-9590-47ab-939b-926fa5c77459" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Password", "SecurityCode", "SecurityCodeExpirationDate", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "ftt@yahoo.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1501CAB6-CA3F-470F-AE5E-1A0B970D1707", "fzt", "68596c86-04fc-4e29-a94e-dd0857143397" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Password", "SecurityCode", "SecurityCodeExpirationDate", "Subject", "UserName", "Version" },
                values: new object[] { new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "bsmith@gmail.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "157BBC69-9989-4353-A4B9-02A205678562", "bob", "428595be-7cb5-4e9b-9b90-3896e6bf30cc" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "Type", "UserId", "Value", "Version" },
                values: new object[,]
                {
                    { new Guid("bf88af3a-3724-4ccb-accd-cef5163dceb9"), "name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang Fang", "27cfacd6-9dfa-4d19-a4f1-82a2485d54e0" },
                    { new Guid("dc26a3d0-9d66-48b0-91fb-f2af02ea506e"), "email", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "bsmith@gmail.com", "0f3e5818-6f9e-4e64-a687-a8afc3abc140" },
                    { new Guid("e2fcc50e-d2d1-424e-bbef-560f4a7fd790"), "family_name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Smith", "bfaf9cae-a312-4337-b505-cf17e906c32a" },
                    { new Guid("411c5fb8-2e64-43ea-8552-cf1f3781c25b"), "given_name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob", "621f1c98-4d5f-454d-85be-72c6cdfe6ddd" },
                    { new Guid("bb8d6fc6-5668-4907-8343-d93de67962f5"), "name", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "Bob Smith", "4c8349af-1aae-4ab2-a6b3-6759ca762028" },
                    { new Guid("0789bd56-0c75-41da-8c3e-830a8f9ec822"), "birthdate", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "1999-09-30", "4c4e3c62-f820-4419-bf26-b62f1c83249b" },
                    { new Guid("cbf55bdb-ba9a-4cad-b4fa-345bcf1786a2"), "address", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "29 Spencer Ave.", "0886ae79-da8f-45e7-b281-cbe52c3adeee" },
                    { new Guid("e533e2a5-6307-4623-ad73-178ac4104844"), "email", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "ftt@yahoo.com", "6307616c-9c5d-4a87-8d6e-6214e0002673" },
                    { new Guid("c224d757-842f-416c-8dbd-cf7e58333a09"), "family_name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Fang", "9b23b5a1-2b43-4fbe-a5c1-bc629ec30db0" },
                    { new Guid("6bce6677-292c-4d04-ad18-6c79c6c35fda"), "given_name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao", "dbb07efd-055e-4778-b8a0-46b7bb97f931" },
                    { new Guid("2b29cedb-b2b5-43d8-9ac4-084d4efe89f3"), "name", new Guid("303cac10-a6c6-4b46-846b-aa07a8d46393"), "Zhitao Fang", "a532a5ea-38ff-4832-a4a8-22ba0770feb2" },
                    { new Guid("923ebff7-ef5c-405f-b940-5bfcf8ab4ba7"), "birthdate", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "1967-03-13", "6dfc99a8-e780-47a9-8b7e-8e78c2d71f44" },
                    { new Guid("3694d258-e26f-4459-a2cc-825b87e20bc0"), "address", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "1750 Bloor St.", "c6920ba6-a651-40a7-9b0f-eaef28d814d0" },
                    { new Guid("beed829e-b9c1-4cd7-896a-7634e0b6b64d"), "email", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "fchy@yahoo.com", "f6f0b9bc-d470-4751-be27-74ab668d84d0" },
                    { new Guid("a266e9ac-df0c-4daa-8361-3aa5a2429320"), "family_name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Fang", "f733d6f9-042b-4221-87f4-e55541f5913a" },
                    { new Guid("81f460b0-32d6-40f2-95ab-8afd34dc8820"), "given_name", new Guid("08e54ef4-f64c-442b-bd4d-63df65bdfb55"), "Chaoyang", "43725c40-cec3-453c-9312-f611e693fc62" },
                    { new Guid("8e15f9ef-132c-4677-bd3b-10edaf95838d"), "address", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "3345 Cardross Rd.", "db65b440-9bcd-4d19-8fcc-96084e28bee2" },
                    { new Guid("db7ef018-eba4-409e-b278-c5fceeac0673"), "birthdate", new Guid("3dd54a32-688f-40fb-9f5e-666ff007b3c1"), "1987-02-23", "a3750e1a-135e-407c-bc12-bbf0b7b3c925" }
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
                unique: true);
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
