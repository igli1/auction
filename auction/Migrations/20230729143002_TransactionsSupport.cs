using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auction.Migrations
{
    /// <inheritdoc />
    public partial class TransactionsSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldItem_AspNetUsers_Buyerid",
                table: "SoldItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldItem_AspNetUsers_SellerId",
                table: "SoldItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldItem_Products_ProductId",
                table: "SoldItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldItem_Transactions_TransactionId",
                table: "SoldItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldItem",
                table: "SoldItem");

            migrationBuilder.RenameTable(
                name: "SoldItem",
                newName: "SoldItems");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItem_TransactionId",
                table: "SoldItems",
                newName: "IX_SoldItems_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItem_SellerId",
                table: "SoldItems",
                newName: "IX_SoldItems_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItem_ProductId",
                table: "SoldItems",
                newName: "IX_SoldItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItem_Buyerid",
                table: "SoldItems",
                newName: "IX_SoldItems_Buyerid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldItems",
                table: "SoldItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItems_AspNetUsers_Buyerid",
                table: "SoldItems",
                column: "Buyerid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItems_AspNetUsers_SellerId",
                table: "SoldItems",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItems_Products_ProductId",
                table: "SoldItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItems_Transactions_TransactionId",
                table: "SoldItems",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldItems_AspNetUsers_Buyerid",
                table: "SoldItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldItems_AspNetUsers_SellerId",
                table: "SoldItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldItems_Products_ProductId",
                table: "SoldItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldItems_Transactions_TransactionId",
                table: "SoldItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldItems",
                table: "SoldItems");

            migrationBuilder.RenameTable(
                name: "SoldItems",
                newName: "SoldItem");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItems_TransactionId",
                table: "SoldItem",
                newName: "IX_SoldItem_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItems_SellerId",
                table: "SoldItem",
                newName: "IX_SoldItem_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItems_ProductId",
                table: "SoldItem",
                newName: "IX_SoldItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItems_Buyerid",
                table: "SoldItem",
                newName: "IX_SoldItem_Buyerid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldItem",
                table: "SoldItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItem_AspNetUsers_Buyerid",
                table: "SoldItem",
                column: "Buyerid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItem_AspNetUsers_SellerId",
                table: "SoldItem",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItem_Products_ProductId",
                table: "SoldItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItem_Transactions_TransactionId",
                table: "SoldItem",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
