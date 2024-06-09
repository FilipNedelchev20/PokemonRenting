using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonRenting.Repositories.Migrations
{
    public partial class UpdatePokemonRental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Rental",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentalStatus",
                table: "Rental",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyRate",
                table: "Pokemons",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "RentalStatus",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "DailyRate",
                table: "Pokemons");
        }
    }
}
