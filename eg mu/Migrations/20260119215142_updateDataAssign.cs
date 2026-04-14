using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eg_mu.Migrations
{
    /// <inheritdoc />
    public partial class updateDataAssign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admins__719FE4E89CBE3E34", x => x.AdminID);
                });

            migrationBuilder.CreateTable(
                name: "MuseumSections",
                columns: table => new
                {
                    SectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MuseumSe__80EF08927B905AA5", x => x.SectionID);
                });

            migrationBuilder.CreateTable(
                name: "TicketPrices",
                columns: table => new
                {
                    PriceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TicketPr__4957584FDE104E7D", x => x.PriceID);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    VisitorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Visitors__B121AFA825C4740C", x => x.VisitorID);
                });

            migrationBuilder.CreateTable(
                name: "ConservationLabs",
                columns: table => new
                {
                    LabID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Conserva__EDBD773A2BE53AA7", x => x.LabID);
                    table.ForeignKey(
                        name: "FK_ConservationLabs_Sections",
                        column: x => x.SectionID,
                        principalTable: "MuseumSections",
                        principalColumn: "SectionID");
                });

            migrationBuilder.CreateTable(
                name: "Exhibitions",
                columns: table => new
                {
                    ExhibitionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exhibiti__32CDCC7EEC518E2C", x => x.ExhibitionID);
                    table.ForeignKey(
                        name: "FK_Exhibitions_Sections",
                        column: x => x.SectionID,
                        principalTable: "MuseumSections",
                        principalColumn: "SectionID");
                });

            migrationBuilder.CreateTable(
                name: "Gardens",
                columns: table => new
                {
                    GardenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SectionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Gardens__0191D063819C7315", x => x.GardenID);
                    table.ForeignKey(
                        name: "FK_Gardens_Sections",
                        column: x => x.SectionID,
                        principalTable: "MuseumSections",
                        principalColumn: "SectionID");
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SectionID = table.Column<int>(type: "int", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Staff__96D4AAF7F933DF4A", x => x.StaffID);
                    table.ForeignKey(
                        name: "FK_Staff_Sections",
                        column: x => x.SectionID,
                        principalTable: "MuseumSections",
                        principalColumn: "SectionID");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorID = table.Column<int>(type: "int", nullable: false),
                    PriceID = table.Column<int>(type: "int", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsStudent = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    AgeCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    VisitDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookings__73951ACD8464F14E", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Bookings_Prices",
                        column: x => x.PriceID,
                        principalTable: "TicketPrices",
                        principalColumn: "PriceID");
                    table.ForeignKey(
                        name: "FK_Bookings_Visitors",
                        column: x => x.VisitorID,
                        principalTable: "Visitors",
                        principalColumn: "VisitorID");
                });

            migrationBuilder.CreateTable(
                name: "Artifacts",
                columns: table => new
                {
                    ArtifactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Era = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SectionID = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Artifact__E788EA9606BA700C", x => x.ArtifactID);
                    table.ForeignKey(
                        name: "FK_Artifacts_ConservationLabs",
                        column: x => x.LabID,
                        principalTable: "ConservationLabs",
                        principalColumn: "LabID");
                    table.ForeignKey(
                        name: "FK_Artifacts_Sections",
                        column: x => x.SectionID,
                        principalTable: "MuseumSections",
                        principalColumn: "SectionID");
                });

            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffID = table.Column<int>(type: "int", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CheckOut = table.Column<DateTime>(type: "datetime", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CheckOutTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Attendan__8B69263CEF699885", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendance_Staff",
                        column: x => x.StaffID,
                        principalTable: "Staff",
                        principalColumn: "StaffID");
                });

            migrationBuilder.CreateTable(
                name: "DailyTasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffID = table.Column<int>(type: "int", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    TaskDate = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DailyTas__7C6949D119972A8D", x => x.TaskID);
                    table.ForeignKey(
                        name: "FK_Tasks_Staff",
                        column: x => x.StaffID,
                        principalTable: "Staff",
                        principalColumn: "StaffID");
                });

            migrationBuilder.CreateTable(
                name: "ArtifactsExhibitions",
                columns: table => new
                {
                    ArtifactID = table.Column<int>(type: "int", nullable: false),
                    ExhibitionID = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Artifact__14A43651826F0E4A", x => new { x.ArtifactID, x.ExhibitionID });
                    table.ForeignKey(
                        name: "FK__Artifacts__Artif__6477ECF3",
                        column: x => x.ArtifactID,
                        principalTable: "Artifacts",
                        principalColumn: "ArtifactID");
                    table.ForeignKey(
                        name: "FK__Artifacts__Exhib__656C112C",
                        column: x => x.ExhibitionID,
                        principalTable: "Exhibitions",
                        principalColumn: "ExhibitionID");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Admins__A9D10534651BC863",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_LabID",
                table: "Artifacts",
                column: "LabID");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_SectionID",
                table: "Artifacts",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactsExhibitions_ExhibitionID",
                table: "ArtifactsExhibitions",
                column: "ExhibitionID");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StaffID",
                table: "Attendance",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PriceID",
                table: "Bookings",
                column: "PriceID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VisitorID",
                table: "Bookings",
                column: "VisitorID");

            migrationBuilder.CreateIndex(
                name: "IX_ConservationLabs_SectionID",
                table: "ConservationLabs",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_StaffID",
                table: "DailyTasks",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_Exhibitions_SectionID",
                table: "Exhibitions",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Gardens_SectionID",
                table: "Gardens",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_SectionID",
                table: "Staff",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "UQ__Staff__A9D10534DD440527",
                table: "Staff",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Visitors__A9D10534558396D0",
                table: "Visitors",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ArtifactsExhibitions");

            migrationBuilder.DropTable(
                name: "Attendance");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "DailyTasks");

            migrationBuilder.DropTable(
                name: "Gardens");

            migrationBuilder.DropTable(
                name: "Artifacts");

            migrationBuilder.DropTable(
                name: "Exhibitions");

            migrationBuilder.DropTable(
                name: "TicketPrices");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "ConservationLabs");

            migrationBuilder.DropTable(
                name: "MuseumSections");
        }
    }
}
