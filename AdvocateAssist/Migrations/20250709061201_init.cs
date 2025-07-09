using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdvocateAssist.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Case",
                columns: table => new
                {
                    CaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Case__6CAE524CC2674450", x => x.CaseId);
                });

            migrationBuilder.CreateTable(
                name: "LegalAssistant",
                columns: table => new
                {
                    LegalAssistantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalAssistantFName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LegalAssistantLName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MonthlyStipend = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NidNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BarLicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LegalAss__C876325AA5C50EDD", x => x.LegalAssistantId);
                });

            migrationBuilder.CreateTable(
                name: "LegalAssistantCase",
                columns: table => new
                {
                    LegalAssistantCaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FIRNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FilingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LegalAssistantId = table.Column<int>(type: "int", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LegalAss__8C4D842CFFEBE815", x => x.LegalAssistantCaseId);
                    table.ForeignKey(
                        name: "FK__LegalAssi__CaseI__286302EC",
                        column: x => x.CaseId,
                        principalTable: "Case",
                        principalColumn: "CaseId");
                    table.ForeignKey(
                        name: "FK__LegalAssi__Legal__29572725",
                        column: x => x.LegalAssistantId,
                        principalTable: "LegalAssistant",
                        principalColumn: "LegalAssistantId");
                });

            migrationBuilder.InsertData(
                table: "Case",
                columns: new[] { "CaseId", "CaseNumber" },
                values: new object[,]
                {
                    { 1, "CASE-2024-01" },
                    { 2, "CASE-2024-02" }
                });

            migrationBuilder.InsertData(
                table: "LegalAssistant",
                columns: new[] { "LegalAssistantId", "BarLicenseNumber", "City", "District", "Division", "Email", "IsActive", "JoinDate", "LegalAssistantFName", "LegalAssistantLName", "MobileNo", "MonthlyStipend", "NidNumber", "Picture" },
                values: new object[,]
                {
                    { 1, "BAR1234", "Mirpur", "Dhaka", "Dhaka", "jony@gmail.com", true, new DateTime(2022, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jony", "Sarowar", "01710000001", 15000m, "1234567890", "jony.jpg" },
                    { 2, "BAR5678", "Agrabad", "Chittagong", "Chittagong", "rafiq@gmail.com", true, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rafiq", "Hasan", "01710000002", 12000m, "1234567890", "rafiq.png" }
                });

            migrationBuilder.InsertData(
                table: "LegalAssistantCase",
                columns: new[] { "LegalAssistantCaseId", "CaseId", "CaseTitle", "FilingDate", "FIRNumber", "LegalAssistantId" },
                values: new object[,]
                {
                    { 1, 1, "Land Dispute Resolution", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "FIR-001", 1 },
                    { 2, 2, "Family Property Claim", new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "FIR-002", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegalAssistantCase_CaseId",
                table: "LegalAssistantCase",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalAssistantCase_LegalAssistantId",
                table: "LegalAssistantCase",
                column: "LegalAssistantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalAssistantCase");

            migrationBuilder.DropTable(
                name: "Case");

            migrationBuilder.DropTable(
                name: "LegalAssistant");
        }
    }
}
