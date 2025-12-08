using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveTracker.Migrations
{
    /// <inheritdoc />
    public partial class updatedefaultdaysdatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DefaultDays",
                table: "LeaveTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DefaultDays",
                table: "LeaveTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
