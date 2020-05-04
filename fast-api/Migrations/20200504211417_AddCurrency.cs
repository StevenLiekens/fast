using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fast_api.Migrations
{
    public partial class AddCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "SelectionContainers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "SelectionContainerItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "SelectionContainerItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrencyTrade",
                columns: table => new
                {
                    CurrencyTradeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    CoinCost = table.Column<int>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: true),
                    ItemAmount = table.Column<int>(nullable: false),
                    SelectionContainerId = table.Column<int>(nullable: true),
                    SelectionContainerAmount = table.Column<int>(nullable: false),
                    Buy = table.Column<int>(nullable: false),
                    Sell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTrade", x => x.CurrencyTradeId);
                    table.ForeignKey(
                        name: "FK_CurrencyTrade_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyTrade_SelectionContainers_SelectionContainerId",
                        column: x => x.SelectionContainerId,
                        principalTable: "SelectionContainers",
                        principalColumn: "SelectionContainerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyTradeCost",
                columns: table => new
                {
                    CurrencyTradeCostId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Currency = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    CurrencyTradeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTradeCost", x => x.CurrencyTradeCostId);
                    table.ForeignKey(
                        name: "FK_CurrencyTradeCost_CurrencyTrade_CurrencyTradeId",
                        column: x => x.CurrencyTradeId,
                        principalTable: "CurrencyTrade",
                        principalColumn: "CurrencyTradeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrade_ItemId",
                table: "CurrencyTrade",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrade_SelectionContainerId",
                table: "CurrencyTrade",
                column: "SelectionContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTradeCost_CurrencyTradeId",
                table: "CurrencyTradeCost",
                column: "CurrencyTradeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyTradeCost");

            migrationBuilder.DropTable(
                name: "CurrencyTrade");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "SelectionContainers");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "SelectionContainerItems");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "SelectionContainerItems");
        }
    }
}
