using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminData.Migrations
{
    /// <inheritdoc />
    public partial class teo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_Category_CategoryId",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_CategoryId",
                table: "product");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "product",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId1",
                table: "product",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_product_CategoryId1",
                table: "product",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_product_Category_CategoryId1",
                table: "product",
                column: "CategoryId1",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_Category_CategoryId1",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_CategoryId1",
                table: "product");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "product");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "product",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_product_CategoryId",
                table: "product",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_product_Category_CategoryId",
                table: "product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
