using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LDCT.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalRecordNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    ExamDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Track1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Track2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Track3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Track4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChestClinicOneMonth = table.Column<bool>(type: "bit", nullable: false),
                    Track3Months = table.Column<bool>(type: "bit", nullable: false),
                    Track6Months = table.Column<bool>(type: "bit", nullable: false),
                    Track12Months = table.Column<bool>(type: "bit", nullable: false),
                    LdctStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClosureCode = table.Column<int>(type: "int", nullable: true),
                    ClosedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ClosedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasNodule = table.Column<bool>(type: "bit", nullable: true),
                    NoduleCount = table.Column<int>(type: "int", nullable: true),
                    MaxNoduleLengthMm = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NeedOutpatientFollowUp = table.Column<bool>(type: "bit", nullable: true),
                    ReportNarrativeSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalReportQueryUrlTemplateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportLanded = table.Column<bool>(type: "bit", nullable: false),
                    ReportLandedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LlmRawOutputJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LlmVerifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LlmNeedsReview = table.Column<bool>(type: "bit", nullable: false),
                    CaseManagerNarrativeNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisRegistered = table.Column<bool>(type: "bit", nullable: false),
                    DiagnosisStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationSettings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "OrphanLdctReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalRecordNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PayloadSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrphanLdctReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FollowUpLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackCorridor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ContactResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedSmsMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HisQuerySummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PreviousSnapshotJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUpLogs_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderMessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsMessages_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_MedicalRecordNumber",
                table: "Cases",
                column: "MedicalRecordNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpLogs_CaseId_ContactDate",
                table: "FollowUpLogs",
                columns: new[] { "CaseId", "ContactDate" });

            migrationBuilder.CreateIndex(
                name: "IX_OrphanLdctReports_ReceivedAt",
                table: "OrphanLdctReports",
                column: "ReceivedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SmsMessages_CaseId",
                table: "SmsMessages",
                column: "CaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUpLogs");

            migrationBuilder.DropTable(
                name: "IntegrationSettings");

            migrationBuilder.DropTable(
                name: "OrphanLdctReports");

            migrationBuilder.DropTable(
                name: "SmsMessages");

            migrationBuilder.DropTable(
                name: "Cases");
        }
    }
}
