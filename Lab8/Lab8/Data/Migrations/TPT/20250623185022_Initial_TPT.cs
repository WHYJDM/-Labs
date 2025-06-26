using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab8.Data.Migrations.TPT
{
    /// <inheritdoc />
    public partial class Initial_TPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlcoholicDrinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlcoholPercentage = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlcoholicDrinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlcoholicDrinks_Drinks_Id",
                        column: x => x.Id,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NonAlcoholicDrinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SugarPer100ml = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonAlcoholicDrinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonAlcoholicDrinks_Drinks_Id",
                        column: x => x.Id,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlcoholicDrinks");

            migrationBuilder.DropTable(
                name: "NonAlcoholicDrinks");

            migrationBuilder.DropTable(
                name: "Drinks");
        }
    }
}
