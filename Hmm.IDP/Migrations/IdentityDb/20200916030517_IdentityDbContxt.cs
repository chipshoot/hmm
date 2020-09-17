using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hmm.IDP.Migrations.IdentityDb
{
    public partial class IdentityDbContxt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<byte[]>(nullable: false),
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
                    Id = table.Column<byte[]>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false),
                    UserId = table.Column<byte[]>(nullable: false)
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
                values: new object[] { new byte[] { 244, 78, 229, 8, 76, 246, 43, 68, 189, 77, 99, 223, 101, 189, 251, 85 }, "fchy@yahoo.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E", "fchy", "f37bea81-91fb-4b13-9b89-d0173fdb96e6" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Password", "SecurityCode", "SecurityCodeExpirationDate", "Subject", "UserName", "Version" },
                values: new object[] { new byte[] { 16, 172, 60, 48, 198, 166, 70, 75, 132, 107, 170, 7, 168, 212, 99, 147 }, "ftt@yahoo.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1501CAB6-CA3F-470F-AE5E-1A0B970D1707", "fzt", "90ae73ca-5157-4c45-9573-a589f155babd" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Password", "SecurityCode", "SecurityCodeExpirationDate", "Subject", "UserName", "Version" },
                values: new object[] { new byte[] { 50, 74, 213, 61, 143, 104, 251, 64, 159, 94, 102, 111, 240, 7, 179, 193 }, "bsmith@gmail.com", true, "AQAAAAEAACcQAAAAEG/4LGAH+5+zQRO3cPWA/um+2U/BiFudtLhUi29npPzYa1wCdbfOBb+WzoEwFlOMHg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "157BBC69-9989-4353-A4B9-02A205678562", "bob", "36f77b12-04c4-4151-b0ba-44da88a986b5" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "Type", "UserId", "Value", "Version" },
                values: new object[,]
                {
                    { new byte[] { 48, 119, 1, 202, 211, 115, 174, 76, 186, 33, 51, 38, 126, 167, 126, 173 }, "name", new byte[] { 244, 78, 229, 8, 76, 246, 43, 68, 189, 77, 99, 223, 101, 189, 251, 85 }, "Chaoyang Fang", "2cbb25ca-cde9-4190-825f-07c2024510f3" },
                    { new byte[] { 106, 239, 15, 105, 138, 133, 75, 69, 178, 148, 184, 5, 149, 182, 219, 14 }, "email", new byte[] { 50, 74, 213, 61, 143, 104, 251, 64, 159, 94, 102, 111, 240, 7, 179, 193 }, "bsmith@gmail.com", "871b3014-1297-4b3a-9166-4a367aabfa1c" },
                    { new byte[] { 199, 230, 154, 243, 247, 159, 220, 64, 174, 167, 189, 6, 81, 67, 228, 124 }, "family_name", new byte[] { 50, 74, 213, 61, 143, 104, 251, 64, 159, 94, 102, 111, 240, 7, 179, 193 }, "Smith", "8538a826-9490-4f12-8cab-555767e829c1" },
                    { new byte[] { 135, 248, 63, 211, 76, 128, 177, 79, 149, 50, 212, 159, 122, 191, 152, 56 }, "given_name", new byte[] { 50, 74, 213, 61, 143, 104, 251, 64, 159, 94, 102, 111, 240, 7, 179, 193 }, "Bob", "f14c4924-b918-426d-818d-2452720adc1b" },
                    { new byte[] { 103, 35, 57, 127, 47, 160, 203, 76, 176, 138, 195, 235, 63, 43, 3, 101 }, "name", new byte[] { 50, 74, 213, 61, 143, 104, 251, 64, 159, 94, 102, 111, 240, 7, 179, 193 }, "Bob Smith", "eb433deb-73d0-4979-a2b9-a27662a4884c" },
                    { new byte[] { 128, 62, 248, 20, 42, 181, 194, 64, 135, 149, 220, 56, 4, 232, 174, 248 }, "birthdate", new byte[] { 16, 172, 60, 48, 198, 166, 70, 75, 132, 107, 170, 7, 168, 212, 99, 147 }, "1999-09-30", "a8685044-3dec-4c38-8634-26df92c6eba2" },
                    { new byte[] { 158, 40, 17, 30, 58, 86, 247, 75, 138, 197, 96, 232, 90, 191, 42, 125 }, "address", new byte[] { 16, 172, 60, 48, 198, 166, 70, 75, 132, 107, 170, 7, 168, 212, 99, 147 }, "29 Spencer Ave.", "1b57ef04-00c5-4b02-9bff-8869f5310184" },
                    { new byte[] { 206, 64, 110, 177, 61, 252, 130, 77, 140, 234, 221, 219, 5, 126, 37, 122 }, "email", new byte[] { 16, 172, 60, 48, 198, 166, 70, 75, 132, 107, 170, 7, 168, 212, 99, 147 }, "ftt@yahoo.com", "1eacd436-7e6f-4e01-9b3b-f1d4bbc16a34" },
                    { new byte[] { 233, 101, 146, 76, 43, 113, 242, 67, 148, 174, 210, 13, 124, 46, 46, 139 }, "family_name", new byte[] { 16, 172, 60, 48, 198, 166, 70, 75, 132, 107, 170, 7, 168, 212, 99, 147 }, "Fang", "0115e83c-28f2-49a1-adb7-ebacf07461b9" },
                    { new byte[] { 78, 229, 192, 246, 151, 173, 113, 64, 142, 10, 170, 39, 185, 91, 16, 154 }, "given_name", new byte[] { 16, 172, 60, 48, 198, 166, 70, 75, 132, 107, 170, 7, 168, 212, 99, 147 }, "Zhitao", "e37ec180-87b6-41d5-932c-e352e9bd5f56" },
                    { new byte[] { 119, 122, 124, 174, 38, 172, 220, 70, 177, 184, 140, 199, 209, 131, 253, 200 }, "name", new byte[] { 16, 172, 60, 48, 198, 166, 70, 75, 132, 107, 170, 7, 168, 212, 99, 147 }, "Zhitao Fang", "53ea166c-6c77-4c9d-baa9-418b5cb05e12" },
                    { new byte[] { 13, 177, 108, 78, 44, 210, 153, 73, 170, 164, 143, 213, 144, 239, 145, 218 }, "birthdate", new byte[] { 244, 78, 229, 8, 76, 246, 43, 68, 189, 77, 99, 223, 101, 189, 251, 85 }, "1967-03-13", "2125ab14-5982-4c19-95cc-7bb60ab0436e" },
                    { new byte[] { 41, 45, 41, 253, 168, 158, 34, 78, 171, 171, 176, 194, 4, 83, 197, 179 }, "address", new byte[] { 244, 78, 229, 8, 76, 246, 43, 68, 189, 77, 99, 223, 101, 189, 251, 85 }, "1750 Bloor St.", "4d80861f-4f36-441b-a07f-2763d174e832" },
                    { new byte[] { 71, 195, 125, 204, 227, 255, 77, 70, 177, 62, 241, 212, 107, 173, 219, 89 }, "email", new byte[] { 244, 78, 229, 8, 76, 246, 43, 68, 189, 77, 99, 223, 101, 189, 251, 85 }, "fchy@yahoo.com", "e97b5905-b881-404a-bb98-9ada4c5bb3ba" },
                    { new byte[] { 219, 248, 128, 85, 72, 217, 79, 75, 183, 128, 55, 250, 179, 44, 21, 67 }, "family_name", new byte[] { 244, 78, 229, 8, 76, 246, 43, 68, 189, 77, 99, 223, 101, 189, 251, 85 }, "Fang", "9efa47eb-3ac8-4c54-8d56-e4f604acb6ec" },
                    { new byte[] { 101, 223, 52, 251, 112, 20, 189, 66, 154, 48, 18, 38, 172, 222, 54, 142 }, "given_name", new byte[] { 244, 78, 229, 8, 76, 246, 43, 68, 189, 77, 99, 223, 101, 189, 251, 85 }, "Chaoyang", "50c7d2dc-667b-4e5f-acf9-576d42465c4e" },
                    { new byte[] { 134, 213, 16, 97, 174, 74, 57, 68, 166, 179, 31, 200, 160, 79, 130, 194 }, "address", new byte[] { 50, 74, 213, 61, 143, 104, 251, 64, 159, 94, 102, 111, 240, 7, 179, 193 }, "3345 Cardross Rd.", "ed5311b8-b757-47de-ab2b-614e089bdb51" },
                    { new byte[] { 184, 70, 107, 142, 235, 46, 0, 68, 173, 255, 143, 108, 48, 58, 84, 191 }, "birthdate", new byte[] { 50, 74, 213, 61, 143, 104, 251, 64, 159, 94, 102, 111, 240, 7, 179, 193 }, "1987-02-23", "56ddf4a6-d0f1-4120-8c99-c01d3482e790" }
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
