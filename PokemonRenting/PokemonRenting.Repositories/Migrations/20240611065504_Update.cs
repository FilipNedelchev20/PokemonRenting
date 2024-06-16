using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonRenting.Repositories.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrivingLicence",
                table: "UserDetails",
                newName: "TrainerLevel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainerLevel",
                table: "UserDetails",
                newName: "DrivingLicence");
        }
    }
}
