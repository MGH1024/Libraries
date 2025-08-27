using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "sec",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "sec",
                table: "UserOperationClaims",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "sec",
                table: "RefreshTokens",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "sec",
                table: "PolicyOperationClaims",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "sec",
                table: "Policies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "sec",
                table: "OperationClaims",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                schema: "sec",
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: 0L);

            migrationBuilder.UpdateData(
                schema: "sec",
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: 0L);

            migrationBuilder.UpdateData(
                schema: "sec",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt", "Version" },
                values: new object[] { new byte[] { 229, 233, 219, 67, 210, 6, 142, 168, 180, 148, 192, 234, 231, 39, 220, 68, 247, 205, 241, 120, 217, 214, 89, 142, 85, 76, 0, 182, 253, 240, 201, 176, 98, 109, 164, 183, 4, 186, 159, 176, 63, 44, 42, 138, 19, 5, 24, 178, 117, 215, 214, 36, 121, 79, 13, 26, 31, 24, 170, 197, 39, 192, 243, 216 }, new byte[] { 108, 247, 231, 95, 59, 75, 217, 83, 216, 252, 162, 29, 92, 191, 139, 29, 33, 133, 128, 232, 147, 167, 169, 32, 233, 234, 163, 154, 127, 253, 36, 233, 206, 144, 150, 118, 143, 8, 77, 242, 48, 227, 188, 120, 89, 210, 39, 203, 147, 238, 186, 35, 198, 172, 234, 175, 10, 9, 200, 209, 171, 83, 223, 28, 64, 81, 31, 17, 64, 251, 178, 246, 248, 71, 214, 245, 7, 63, 20, 233, 47, 26, 82, 254, 192, 70, 61, 247, 244, 152, 97, 17, 59, 238, 189, 208, 234, 54, 70, 165, 198, 85, 187, 208, 109, 177, 6, 167, 89, 105, 13, 104, 63, 91, 94, 111, 18, 93, 224, 112, 234, 222, 107, 174, 130, 211, 45, 163 }, 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "sec",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "sec",
                table: "UserOperationClaims");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "sec",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "sec",
                table: "PolicyOperationClaims");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "sec",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "sec",
                table: "OperationClaims");

            migrationBuilder.UpdateData(
                schema: "sec",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 42, 65, 194, 146, 203, 147, 145, 161, 110, 154, 34, 120, 169, 59, 67, 41, 100, 171, 182, 117, 229, 56, 244, 224, 59, 51, 178, 68, 250, 232, 207, 210, 211, 161, 71, 151, 117, 196, 161, 148, 226, 178, 207, 216, 56, 178, 85, 132, 240, 252, 195, 14, 70, 222, 21, 6, 194, 55, 67, 209, 159, 119, 204, 162 }, new byte[] { 217, 53, 73, 136, 167, 156, 216, 49, 41, 229, 138, 62, 242, 13, 173, 156, 100, 75, 123, 200, 171, 160, 61, 68, 179, 38, 198, 201, 96, 210, 244, 30, 131, 26, 121, 186, 110, 238, 229, 210, 134, 28, 0, 16, 54, 65, 87, 210, 3, 5, 105, 30, 161, 105, 63, 10, 226, 76, 218, 216, 183, 29, 222, 132, 125, 190, 121, 5, 202, 51, 64, 189, 53, 9, 100, 222, 81, 103, 161, 60, 215, 104, 205, 203, 244, 237, 206, 198, 230, 103, 129, 187, 92, 196, 116, 138, 139, 141, 248, 104, 201, 130, 129, 1, 160, 116, 200, 227, 145, 23, 193, 124, 131, 248, 214, 198, 182, 30, 165, 155, 39, 52, 64, 66, 115, 228, 96, 226 } });
        }
    }
}
