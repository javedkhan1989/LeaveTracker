using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveTracker.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Types_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Types",
                table: "Types");

            migrationBuilder.RenameTable(
                name: "Types",
                newName: "LeaveTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes");

            migrationBuilder.RenameTable(
                name: "LeaveTypes",
                newName: "Types");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Types_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "Types",
                principalColumn: "LeaveTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
