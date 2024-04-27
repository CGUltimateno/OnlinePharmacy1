using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlinePharmacy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicneRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Patients_PatientInfoUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_PatientInfoUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PatientInfoUserId",
                table: "Requests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientInfoUserId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PatientInfoUserId",
                table: "Requests",
                column: "PatientInfoUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Patients_PatientInfoUserId",
                table: "Requests",
                column: "PatientInfoUserId",
                principalTable: "Patients",
                principalColumn: "UserId");
        }
    }
}
