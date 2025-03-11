using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AGENCES",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NUMERO = table.Column<string>(maxLength: 50, nullable: false),
                    NOM = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGENCES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FOURNITURES",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOM = table.Column<string>(maxLength: 100, nullable: false),
                    DATE = table.Column<DateTime>(nullable: false),
                    AGENCE_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FOURNITURES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FOURNITURES_AGENCES_AGENCE_ID",
                        column: x => x.AGENCE_ID,
                        principalTable: "AGENCES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOM = table.Column<string>(maxLength: 100, nullable: false),
                    PRENOM = table.Column<string>(maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(maxLength: 100, nullable: false),
                    MOT_DE_PASSE = table.Column<string>(nullable: false),
                    AGENCE_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USERS_AGENCES_AGENCE_ID",
                        column: x => x.AGENCE_ID,
                        principalTable: "AGENCES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AGENCES_NUMERO",
                table: "AGENCES",
                column: "NUMERO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FOURNITURES_AGENCE_ID",
                table: "FOURNITURES",
                column: "AGENCE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_AGENCE_ID",
                table: "USERS",
                column: "AGENCE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_EMAIL",
                table: "USERS",
                column: "EMAIL",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FOURNITURES");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "AGENCES");
        }
    }
}

