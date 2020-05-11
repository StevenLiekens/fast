using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fast_api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Buy = table.Column<int>(nullable: false),
                    Sell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "SelectionContainers",
                columns: table => new
                {
                    SelectionContainerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    Buy = table.Column<int>(nullable: false),
                    Sell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionContainers", x => x.SelectionContainerId);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyTrades",
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
                    table.PrimaryKey("PK_CurrencyTrades", x => x.CurrencyTradeId);
                    table.ForeignKey(
                        name: "FK_CurrencyTrades_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyTrades_SelectionContainers_SelectionContainerId",
                        column: x => x.SelectionContainerId,
                        principalTable: "SelectionContainers",
                        principalColumn: "SelectionContainerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SelectionContainerItems",
                columns: table => new
                {
                    SelectionContainerId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    Guaranteed = table.Column<bool>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionContainerItems", x => new { x.SelectionContainerId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_SelectionContainerItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectionContainerItems_SelectionContainers_SelectionContain~",
                        column: x => x.SelectionContainerId,
                        principalTable: "SelectionContainers",
                        principalColumn: "SelectionContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyTradeCosts",
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
                    table.PrimaryKey("PK_CurrencyTradeCosts", x => x.CurrencyTradeCostId);
                    table.ForeignKey(
                        name: "FK_CurrencyTradeCosts_CurrencyTrades_CurrencyTradeId",
                        column: x => x.CurrencyTradeId,
                        principalTable: "CurrencyTrades",
                        principalColumn: "CurrencyTradeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTradeCosts_CurrencyTradeId",
                table: "CurrencyTradeCosts",
                column: "CurrencyTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrades_ItemId",
                table: "CurrencyTrades",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrades_SelectionContainerId",
                table: "CurrencyTrades",
                column: "SelectionContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectionContainerItems_ItemId",
                table: "SelectionContainerItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyTradeCosts");

            migrationBuilder.DropTable(
                name: "SelectionContainerItems");

            migrationBuilder.DropTable(
                name: "CurrencyTrades");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "SelectionContainers");
        }
    }
}
