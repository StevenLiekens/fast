using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fast_api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    Buy = table.Column<int>(nullable: false),
                    Sell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    ContainerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Buy = table.Column<int>(nullable: false),
                    Sell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.ContainerId);
                });

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
                name: "ContainerCategories",
                columns: table => new
                {
                    ContainerId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    DropRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerCategories", x => new { x.ContainerId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ContainerCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContainerCategories_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContainerContainers",
                columns: table => new
                {
                    ParentContainerId = table.Column<int>(nullable: false),
                    ContainerId = table.Column<int>(nullable: false),
                    DropRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerContainers", x => new { x.ParentContainerId, x.ContainerId });
                    table.ForeignKey(
                        name: "FK_ContainerContainers_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContainerContainers_Containers_ParentContainerId",
                        column: x => x.ParentContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContainerCurrencies",
                columns: table => new
                {
                    ContainerId = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    DropRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerCurrencies", x => new { x.ContainerId, x.Currency });
                    table.ForeignKey(
                        name: "FK_ContainerCurrencies_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryItems",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryItems", x => new { x.CategoryId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_CategoryItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContainerItems",
                columns: table => new
                {
                    ContainerId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    DropRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerItems", x => new { x.ContainerId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_ContainerItems_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContainerItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContainerSelectionContainers",
                columns: table => new
                {
                    ContainerId = table.Column<int>(nullable: false),
                    SelectionContainerId = table.Column<int>(nullable: false),
                    DropRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerSelectionContainers", x => new { x.ContainerId, x.SelectionContainerId });
                    table.ForeignKey(
                        name: "FK_ContainerSelectionContainers_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContainerSelectionContainers_SelectionContainers_SelectionCo~",
                        column: x => x.SelectionContainerId,
                        principalTable: "SelectionContainers",
                        principalColumn: "SelectionContainerId",
                        onDelete: ReferentialAction.Cascade);
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
                    SelectionContainerId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ContainerId = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Buy = table.Column<int>(nullable: false),
                    Sell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTrades", x => x.CurrencyTradeId);
                    table.ForeignKey(
                        name: "FK_CurrencyTrades_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyTrades_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "SelectionContainerCategories",
                columns: table => new
                {
                    SelectionContainerId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Guaranteed = table.Column<bool>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionContainerCategories", x => new { x.SelectionContainerId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_SelectionContainerCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectionContainerCategories_SelectionContainers_SelectionCo~",
                        column: x => x.SelectionContainerId,
                        principalTable: "SelectionContainers",
                        principalColumn: "SelectionContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectionContainerContainers",
                columns: table => new
                {
                    SelectionContainerId = table.Column<int>(nullable: false),
                    ContainerId = table.Column<int>(nullable: false),
                    Guaranteed = table.Column<bool>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionContainerContainers", x => new { x.SelectionContainerId, x.ContainerId });
                    table.ForeignKey(
                        name: "FK_SelectionContainerContainers_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectionContainerContainers_SelectionContainers_SelectionCo~",
                        column: x => x.SelectionContainerId,
                        principalTable: "SelectionContainers",
                        principalColumn: "SelectionContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectionContainerCurrencies",
                columns: table => new
                {
                    SelectionContainerId = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    Guaranteed = table.Column<bool>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionContainerCurrencies", x => new { x.SelectionContainerId, x.Currency });
                    table.ForeignKey(
                        name: "FK_SelectionContainerCurrencies_SelectionContainers_SelectionCo~",
                        column: x => x.SelectionContainerId,
                        principalTable: "SelectionContainers",
                        principalColumn: "SelectionContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectionContainerItems",
                columns: table => new
                {
                    SelectionContainerId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
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
                name: "IX_CategoryItems_ItemId",
                table: "CategoryItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerCategories_CategoryId",
                table: "ContainerCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerContainers_ContainerId",
                table: "ContainerContainers",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerItems_ItemId",
                table: "ContainerItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerSelectionContainers_SelectionContainerId",
                table: "ContainerSelectionContainers",
                column: "SelectionContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTradeCosts_CurrencyTradeId",
                table: "CurrencyTradeCosts",
                column: "CurrencyTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrades_CategoryId",
                table: "CurrencyTrades",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrades_ContainerId",
                table: "CurrencyTrades",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrades_ItemId",
                table: "CurrencyTrades",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTrades_SelectionContainerId",
                table: "CurrencyTrades",
                column: "SelectionContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectionContainerCategories_CategoryId",
                table: "SelectionContainerCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectionContainerContainers_ContainerId",
                table: "SelectionContainerContainers",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectionContainerItems_ItemId",
                table: "SelectionContainerItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryItems");

            migrationBuilder.DropTable(
                name: "ContainerCategories");

            migrationBuilder.DropTable(
                name: "ContainerContainers");

            migrationBuilder.DropTable(
                name: "ContainerCurrencies");

            migrationBuilder.DropTable(
                name: "ContainerItems");

            migrationBuilder.DropTable(
                name: "ContainerSelectionContainers");

            migrationBuilder.DropTable(
                name: "CurrencyTradeCosts");

            migrationBuilder.DropTable(
                name: "SelectionContainerCategories");

            migrationBuilder.DropTable(
                name: "SelectionContainerContainers");

            migrationBuilder.DropTable(
                name: "SelectionContainerCurrencies");

            migrationBuilder.DropTable(
                name: "SelectionContainerItems");

            migrationBuilder.DropTable(
                name: "CurrencyTrades");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "SelectionContainers");
        }
    }
}
