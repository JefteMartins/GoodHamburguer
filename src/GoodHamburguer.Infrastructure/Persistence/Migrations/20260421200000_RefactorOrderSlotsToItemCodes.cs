using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodHamburguer.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(GoodHamburguerDbContext))]
    [Migration("20260421200000_RefactorOrderSlotsToItemCodes")]
    public partial class RefactorOrderSlotsToItemCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DrinkItemCode",
                table: "orders",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SandwichItemCode",
                table: "orders",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SideItemCode",
                table: "orders",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql("""
                UPDATE orders
                SET SandwichItemCode = CASE SandwichName
                    WHEN 'X Burger' THEN 'sandwich-x-burger'
                    WHEN 'X Bacon' THEN 'sandwich-x-bacon'
                    WHEN 'X Egg' THEN 'sandwich-x-egg'
                    ELSE NULL
                END,
                SideItemCode = CASE SideName
                    WHEN 'Batata frita' THEN 'side-fries'
                    ELSE NULL
                END,
                DrinkItemCode = CASE DrinkName
                    WHEN 'Refrigerante' THEN 'drink-soft-drink'
                    ELSE NULL
                END;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_orders_DrinkItemCode",
                table: "orders",
                column: "DrinkItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_orders_SandwichItemCode",
                table: "orders",
                column: "SandwichItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_orders_SideItemCode",
                table: "orders",
                column: "SideItemCode");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_menu_items_DrinkItemCode",
                table: "orders",
                column: "DrinkItemCode",
                principalTable: "menu_items",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_menu_items_SandwichItemCode",
                table: "orders",
                column: "SandwichItemCode",
                principalTable: "menu_items",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_menu_items_SideItemCode",
                table: "orders",
                column: "SideItemCode",
                principalTable: "menu_items",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "DrinkName",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DrinkUnitPrice",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "SandwichName",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "SandwichUnitPrice",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "SideName",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "SideUnitPrice",
                table: "orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DrinkName",
                table: "orders",
                type: "varchar(120)",
                maxLength: 120,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "DrinkUnitPrice",
                table: "orders",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SandwichName",
                table: "orders",
                type: "varchar(120)",
                maxLength: 120,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "SandwichUnitPrice",
                table: "orders",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SideName",
                table: "orders",
                type: "varchar(120)",
                maxLength: 120,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "SideUnitPrice",
                table: "orders",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE orders
                SET SandwichName = CASE SandwichItemCode
                    WHEN 'sandwich-x-burger' THEN 'X Burger'
                    WHEN 'sandwich-x-bacon' THEN 'X Bacon'
                    WHEN 'sandwich-x-egg' THEN 'X Egg'
                    ELSE NULL
                END,
                SandwichUnitPrice = CASE SandwichItemCode
                    WHEN 'sandwich-x-burger' THEN 5.00
                    WHEN 'sandwich-x-bacon' THEN 7.00
                    WHEN 'sandwich-x-egg' THEN 4.50
                    ELSE NULL
                END,
                SideName = CASE SideItemCode
                    WHEN 'side-fries' THEN 'Batata frita'
                    ELSE NULL
                END,
                SideUnitPrice = CASE SideItemCode
                    WHEN 'side-fries' THEN 2.00
                    ELSE NULL
                END,
                DrinkName = CASE DrinkItemCode
                    WHEN 'drink-soft-drink' THEN 'Refrigerante'
                    ELSE NULL
                END,
                DrinkUnitPrice = CASE DrinkItemCode
                    WHEN 'drink-soft-drink' THEN 2.50
                    ELSE NULL
                END;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_orders_menu_items_DrinkItemCode",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_menu_items_SandwichItemCode",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_menu_items_SideItemCode",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_DrinkItemCode",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_SandwichItemCode",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_SideItemCode",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DrinkItemCode",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "SandwichItemCode",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "SideItemCode",
                table: "orders");
        }
    }
}
