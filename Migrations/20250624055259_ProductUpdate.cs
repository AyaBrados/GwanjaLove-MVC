using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GwanjaLoveProto.Migrations
{
    /// <inheritdoc />
    public partial class ProductUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopSpecials_Products_ProductId",
                table: "ShopSpecials");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ShopSpecials",
                newName: "SpecialProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopSpecials_ProductId",
                table: "ShopSpecials",
                newName: "IX_ShopSpecials_SpecialProductId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserFavourites",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "UserFavourites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "SurveyResponses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SurveyResponses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserFavouriteId",
                table: "SurveyResponses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "ShopSpecials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ShopSpecials",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "Yield",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ShopSpecialId",
                table: "OrderProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpecialProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SetupUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetupDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavourites_ProductId",
                table: "UserFavourites",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavourites_UserId",
                table: "UserFavourites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_ProductId",
                table: "SurveyResponses",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_UserFavouriteId",
                table: "SurveyResponses",
                column: "UserFavouriteId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopSpecials_CartId",
                table: "ShopSpecials",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ShopSpecialId",
                table: "OrderProducts",
                column: "ShopSpecialId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialProduct_ProductId",
                table: "SpecialProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_ShopSpecials_ShopSpecialId",
                table: "OrderProducts",
                column: "ShopSpecialId",
                principalTable: "ShopSpecials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopSpecials_Cart_CartId",
                table: "ShopSpecials",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopSpecials_SpecialProduct_SpecialProductId",
                table: "ShopSpecials",
                column: "SpecialProductId",
                principalTable: "SpecialProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_Products_ProductId",
                table: "SurveyResponses",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_UserFavourites_UserFavouriteId",
                table: "SurveyResponses",
                column: "UserFavouriteId",
                principalTable: "UserFavourites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavourites_AspNetUsers_UserId",
                table: "UserFavourites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavourites_Products_ProductId",
                table: "UserFavourites",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_ShopSpecials_ShopSpecialId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopSpecials_Cart_CartId",
                table: "ShopSpecials");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopSpecials_SpecialProduct_SpecialProductId",
                table: "ShopSpecials");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_Products_ProductId",
                table: "SurveyResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_UserFavourites_UserFavouriteId",
                table: "SurveyResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavourites_AspNetUsers_UserId",
                table: "UserFavourites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavourites_Products_ProductId",
                table: "UserFavourites");

            migrationBuilder.DropTable(
                name: "SpecialProduct");

            migrationBuilder.DropIndex(
                name: "IX_UserFavourites_ProductId",
                table: "UserFavourites");

            migrationBuilder.DropIndex(
                name: "IX_UserFavourites_UserId",
                table: "UserFavourites");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_ProductId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_UserFavouriteId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_ShopSpecials_CartId",
                table: "ShopSpecials");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_ShopSpecialId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "UserFavourites");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "UserFavouriteId",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "ShopSpecials");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ShopSpecials");

            migrationBuilder.DropColumn(
                name: "Yield",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShopSpecialId",
                table: "OrderProducts");

            migrationBuilder.RenameColumn(
                name: "SpecialProductId",
                table: "ShopSpecials",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopSpecials_SpecialProductId",
                table: "ShopSpecials",
                newName: "IX_ShopSpecials_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserFavourites",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductId",
                table: "OrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopSpecials_Products_ProductId",
                table: "ShopSpecials",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
