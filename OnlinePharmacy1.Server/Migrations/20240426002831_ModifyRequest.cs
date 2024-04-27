using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlinePharmacy.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests"); 

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Requests");


            // Recreate the column with the desired IDENTITY property

            migrationBuilder.AddColumn<string>(
                name: "RequestId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
               name: "PK_Requests",
               table: "Requests",
               column: "RequestId");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                           name: "RequestId",
                           table: "Requests",
                           type: "int",
                           nullable: false,
                           defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
               name: "PK_Requests",
               table: "Requests",
               column: "RequestId");
        }
    }
}
