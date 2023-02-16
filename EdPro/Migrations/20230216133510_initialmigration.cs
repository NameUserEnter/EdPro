using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdPro.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetencesTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompType = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetencesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControlTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlTypeName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EdProgramTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdProgramTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EDBO = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Competence = table.Column<string>(type: "text", nullable: false),
                    CompetenceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competences_CompetencesTypes",
                        column: x => x.CompetenceTypeId,
                        principalTable: "CompetencesTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearningOutcome = table.Column<string>(type: "text", nullable: false),
                    LOname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpecialityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningOutcomes_Specialities",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UniversityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faculties_Universities",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecialityCompetences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialityId = table.Column<int>(type: "int", nullable: false),
                    CompetenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialityCompetences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialityCompetences_Competences",
                        column: x => x.CompetenceId,
                        principalTable: "Competences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpecialityCompetences_Specialities",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EducationPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SpecialityId = table.Column<int>(type: "int", nullable: false),
                    EDBO = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    EdPrTypeId = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    ImplementationDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationPrograms_EdProgramTypes",
                        column: x => x.EdPrTypeId,
                        principalTable: "EdProgramTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationPrograms_Faculties",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationPrograms_Specialities",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EProgramId = table.Column<int>(type: "int", nullable: false),
                    Credit = table.Column<int>(type: "int", nullable: false),
                    ControlId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_ControlTypes",
                        column: x => x.ControlId,
                        principalTable: "ControlTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subjects_EducationPrograms",
                        column: x => x.EProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EpSubjectCompetences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    SpecialityCompetenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpSubjectCompetences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpSubjectCompetences_SpecialityCompetences",
                        column: x => x.SpecialityCompetenceId,
                        principalTable: "SpecialityCompetences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EpSubjectCompetences_Subjects",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EpSubjectLOutcomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    LearningOutcomeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpSubjectLOutcomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpSubjectLOutcomes_LearningOutcomes",
                        column: x => x.LearningOutcomeId,
                        principalTable: "LearningOutcomes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EpSubjectLOutcomes_Subjects",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competences_CompetenceTypeId",
                table: "Competences",
                column: "CompetenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_EdPrTypeId",
                table: "EducationPrograms",
                column: "EdPrTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_FacultyId",
                table: "EducationPrograms",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_SpecialityId",
                table: "EducationPrograms",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_EpSubjectCompetences_SpecialityCompetenceId",
                table: "EpSubjectCompetences",
                column: "SpecialityCompetenceId");

            migrationBuilder.CreateIndex(
                name: "IX_EpSubjectCompetences_SubjectId",
                table: "EpSubjectCompetences",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EpSubjectLOutcomes_LearningOutcomeId",
                table: "EpSubjectLOutcomes",
                column: "LearningOutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_EpSubjectLOutcomes_SubjectId",
                table: "EpSubjectLOutcomes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_UniversityId",
                table: "Faculties",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomes_SpecialityId",
                table: "LearningOutcomes",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialityCompetences_CompetenceId",
                table: "SpecialityCompetences",
                column: "CompetenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialityCompetences_SpecialityId",
                table: "SpecialityCompetences",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ControlId",
                table: "Subjects",
                column: "ControlId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_EProgramId",
                table: "Subjects",
                column: "EProgramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpSubjectCompetences");

            migrationBuilder.DropTable(
                name: "EpSubjectLOutcomes");

            migrationBuilder.DropTable(
                name: "SpecialityCompetences");

            migrationBuilder.DropTable(
                name: "LearningOutcomes");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Competences");

            migrationBuilder.DropTable(
                name: "ControlTypes");

            migrationBuilder.DropTable(
                name: "EducationPrograms");

            migrationBuilder.DropTable(
                name: "CompetencesTypes");

            migrationBuilder.DropTable(
                name: "EdProgramTypes");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "Universities");
        }
    }
}
