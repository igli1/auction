using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auction.Migrations
{
    /// <inheritdoc />
    public partial class TransactionSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldItem_Bids_BidId",
                table: "SoldItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_FromUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_ToUserId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ToUserId",
                table: "Transactions",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "FromUserId",
                table: "Transactions",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ToUserId",
                table: "Transactions",
                newName: "IX_Transactions_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_FromUserId",
                table: "Transactions",
                newName: "IX_Transactions_BuyerId");

            migrationBuilder.RenameColumn(
                name: "BidId",
                table: "SoldItem",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItem_BidId",
                table: "SoldItem",
                newName: "IX_SoldItem_TransactionId");

            migrationBuilder.AddColumn<int>(
                name: "BidId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BidId",
                table: "Transactions",
                column: "BidId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItem_Transactions_TransactionId",
                table: "SoldItem",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_BuyerId",
                table: "Transactions",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_SellerId",
                table: "Transactions",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Bids_BidId",
                table: "Transactions",
                column: "BidId",
                principalTable: "Bids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldItem_Transactions_TransactionId",
                table: "SoldItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_BuyerId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_SellerId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Bids_BidId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BidId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BidId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Transactions",
                newName: "ToUserId");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Transactions",
                newName: "FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SellerId",
                table: "Transactions",
                newName: "IX_Transactions_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BuyerId",
                table: "Transactions",
                newName: "IX_Transactions_FromUserId");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "SoldItem",
                newName: "BidId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItem_TransactionId",
                table: "SoldItem",
                newName: "IX_SoldItem_BidId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItem_Bids_BidId",
                table: "SoldItem",
                column: "BidId",
                principalTable: "Bids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_FromUserId",
                table: "Transactions",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_ToUserId",
                table: "Transactions",
                column: "ToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
