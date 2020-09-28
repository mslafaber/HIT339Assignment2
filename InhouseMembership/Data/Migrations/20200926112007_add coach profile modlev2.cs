using Microsoft.EntityFrameworkCore.Migrations;

namespace InhouseMembership.Data.Migrations
{
    public partial class addcoachprofilemodlev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoachProfiles",
                columns: table => new
                {
                    CoachProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoachId = table.Column<string>(nullable: true),
                    Education = table.Column<string>(maxLength: 160, nullable: false),
                    Interests = table.Column<string>(maxLength: 160, nullable: false),
                    Experience = table.Column<string>(maxLength: 160, nullable: false),
                    Skills = table.Column<string>(maxLength: 160, nullable: false),
                    Biography = table.Column<string>(maxLength: 800, nullable: false),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachProfiles", x => x.CoachProfileId);
                    table.ForeignKey(
                        name: "FK_CoachProfiles_AspNetUsers_CoachId",
                        column: x => x.CoachId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachProfiles_CoachId",
                table: "CoachProfiles",
                column: "CoachId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachProfiles");
        }
    }
}
