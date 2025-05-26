using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileTidyDatabase.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SortFolders",
                columns: table => new
                {
                    GUID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FolderPath = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortFolders", x => x.GUID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicSettings");

            migrationBuilder.DropTable(
                name: "SortFolders");
        }
    }
}
