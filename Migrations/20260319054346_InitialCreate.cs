using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillSwap.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Talents",
                columns: table => new
                {
                    TalentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TalentName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talents", x => x.TalentId);
                    table.ForeignKey(
                        name: "FK_Talents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeRequests",
                columns: table => new
                {
                    TradeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequesterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TargetStudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequestedTalentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OfferedTalentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    TalentEntityTalentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRequests", x => x.TradeId);
                    table.ForeignKey(
                        name: "FK_TradeRequests_Students_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeRequests_Students_TargetStudentId",
                        column: x => x.TargetStudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TradeRequests_Talents_OfferedTalentId",
                        column: x => x.OfferedTalentId,
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeRequests_Talents_RequestedTalentId",
                        column: x => x.RequestedTalentId,
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeRequests_Talents_TalentEntityTalentId",
                        column: x => x.TalentEntityTalentId,
                        principalTable: "Talents",
                        principalColumn: "TalentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Talents_StudentId",
                table: "Talents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRequests_OfferedTalentId",
                table: "TradeRequests",
                column: "OfferedTalentId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRequests_RequestedTalentId",
                table: "TradeRequests",
                column: "RequestedTalentId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRequests_RequesterId",
                table: "TradeRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRequests_TalentEntityTalentId",
                table: "TradeRequests",
                column: "TalentEntityTalentId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRequests_TargetStudentId",
                table: "TradeRequests",
                column: "TargetStudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeRequests");

            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
