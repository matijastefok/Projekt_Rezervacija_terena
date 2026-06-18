using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_Rezervacija_terena.Migrations
{
 
    public partial class DodajKorisnikaURezervaciju : Migration
    {
       
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KorisnikIme",
                table: "Rezervacije",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KorisnikIme",
                table: "Rezervacije");
        }
    }
}
