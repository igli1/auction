using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auction.Migrations
{
    /// <inheritdoc />
    public partial class TimeStampUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Transactions",
                newName: "TransactionTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionTime",
                table: "Transactions",
                newName: "Timestamp");
        }
    }
}
