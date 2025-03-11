using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_oracle.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AGENCES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NUMERO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    NOM = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGENCES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FOURNITURES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOM = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    AGENCE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
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
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOM = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PRENOM = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    MOT_DE_PASSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    AGENCE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
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

        /// <inheritdoc />
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
