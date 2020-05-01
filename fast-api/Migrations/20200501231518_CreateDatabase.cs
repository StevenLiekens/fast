using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fast_api.Migrations
{
    public partial class CreateDatabase : Migration
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
                    Buy = table.Column<int>(nullable: false),
                    Sell = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionContainers", x => x.SelectionContainerId);
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

            migrationBuilder.CreateIndex(
                name: "IX_SelectionContainerItems_ItemId",
                table: "SelectionContainerItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectionContainerItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "SelectionContainers");
        }
    }
}
