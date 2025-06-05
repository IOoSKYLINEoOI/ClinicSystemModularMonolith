using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Employees.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IssuedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IssuedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidUntil = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificates_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLicenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    LicenseNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IssuedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IssuedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidUntil = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLicenses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    RemovedAt = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAssignments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAssignments_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Doctor" },
                    { 2, "Nurse" },
                    { 3, "Receptionist" },
                    { 4, "Administrator" },
                    { 5, "Technician" },
                    { 6, "Surgeon" },
                    { 7, "Pediatrician" },
                    { 8, "Cardiologist" },
                    { 9, "Neurologist" },
                    { 10, "Gynecologist" },
                    { 11, "Oncologist" },
                    { 12, "Dermatologist" },
                    { 13, "OrthopedicSurgeon" },
                    { 14, "Anesthesiologist" },
                    { 15, "Radiologist" },
                    { 16, "Physiotherapist" },
                    { 17, "OccupationalTherapist" },
                    { 18, "MedicalAssistant" },
                    { 19, "LaboratoryTechnician" },
                    { 20, "PharmacyTechnician" },
                    { 21, "HealthAdministrator" },
                    { 22, "Dietitian" },
                    { 23, "ClinicalPsychologist" },
                    { 24, "SocialWorker" },
                    { 25, "Pathologist" },
                    { 26, "Psychiatrist" },
                    { 27, "FamilyDoctor" },
                    { 28, "GeneralPractitioner" },
                    { 29, "Endocrinologist" },
                    { 30, "Urologist" },
                    { 31, "Gastroenterologist" },
                    { 32, "Pulmonologist" },
                    { 33, "Nephrologist" },
                    { 34, "InfectiousDiseaseSpecialist" },
                    { 35, "Hematologist" },
                    { 36, "Rheumatologist" },
                    { 37, "SurgeonAssistant" },
                    { 38, "SurgicalNurse" },
                    { 39, "EmergencyMedicalTechnician" },
                    { 40, "Paramedic" },
                    { 41, "BiomedicalTechnician" },
                    { 42, "RadiologicTechnologist" },
                    { 43, "MedicalEquipmentTechnician" },
                    { 44, "ITSupportSpecialist" },
                    { 45, "HealthInformationTechnician" },
                    { 46, "NetworkAdministrator" },
                    { 47, "MedicalRecordsTechnician" },
                    { 48, "PharmacyTechnologist" },
                    { 49, "LaboratoryManager" },
                    { 50, "QualityControlSpecialist" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignments_EmployeeId",
                table: "EmployeeAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignments_PositionId",
                table: "EmployeeAssignments",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_EmployeeId",
                table: "EmployeeCertificates",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicenses_EmployeeId",
                table: "EmployeeLicenses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAssignments");

            migrationBuilder.DropTable(
                name: "EmployeeCertificates");

            migrationBuilder.DropTable(
                name: "EmployeeLicenses");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
