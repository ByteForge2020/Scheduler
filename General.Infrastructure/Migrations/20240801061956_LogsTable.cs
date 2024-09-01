using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace General.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {  
            migrationBuilder.CreateTable(
            name: "Logs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Timestamp = table.Column<DateTime>(nullable: false),
                Level = table.Column<string>(maxLength: 50, nullable: true),
                Message = table.Column<string>(nullable: true),
                Exception = table.Column<string>(nullable: true),
                Properties = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Logs", x => x.Id);
            });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
