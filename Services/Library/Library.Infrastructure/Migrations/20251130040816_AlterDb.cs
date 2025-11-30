using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Staves",
                schema: "lib",
                newName: "Staves");

            migrationBuilder.RenameTable(
                name: "Authors",
                schema: "lib",
                newName: "Authors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Staves",
                newName: "Staves",
                newSchema: "lib");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "Authors",
                newSchema: "lib");
        }
    }
}
