using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changeinversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new byte[] { 206, 252, 87, 23, 29, 62, 18, 219, 14, 68, 77, 152, 71, 245, 100, 110, 111, 152, 248, 220, 82, 173, 56, 174, 103, 241, 98, 12, 253, 239, 15, 122, 203, 114, 196, 90, 87, 246, 252, 154, 111, 31, 125, 166, 23, 172, 70, 162, 124, 128, 172, 122, 130, 231, 175, 90, 221, 91, 13, 22, 55, 47, 93, 35 }, new byte[] { 245, 220, 228, 132, 129, 20, 161, 50, 205, 162, 248, 250, 209, 196, 211, 8, 233, 105, 20, 56, 99, 116, 173, 4, 221, 94, 253, 182, 89, 45, 91, 255, 179, 229, 186, 80, 201, 104, 1, 10, 96, 179, 94, 101, 192, 175, 29, 67, 45, 87, 97, 203, 137, 8, 251, 198, 2, 89, 214, 95, 162, 87, 131, 185, 86, 61, 231, 143, 195, 118, 20, 251, 154, 207, 127, 155, 140, 43, 108, 97, 94, 185, 253, 53, 39, 9, 129, 122, 62, 130, 229, 146, 178, 132, 142, 226, 178, 11, 136, 69, 189, 35, 139, 83, 164, 28, 158, 71, 132, 169, 146, 82, 53, 143, 44, 197, 117, 35, 208, 82, 62, 26, 89, 179, 156, 229, 211, 89 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
