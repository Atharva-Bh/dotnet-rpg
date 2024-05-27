using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_rpg.Migrations
{
    /// <inheritdoc />
    public partial class Skills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSkill",
                columns: table => new
                {
                    CharactersID = table.Column<int>(type: "int", nullable: false),
                    SkillsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkill", x => new { x.CharactersID, x.SkillsID });
                    table.ForeignKey(
                        name: "FK_CharacterSkill_Characters_CharactersID",
                        column: x => x.CharactersID,
                        principalTable: "Characters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSkill_Skills_SkillsID",
                        column: x => x.SkillsID,
                        principalTable: "Skills",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkill_SkillsID",
                table: "CharacterSkill",
                column: "SkillsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterSkill");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
