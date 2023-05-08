using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EdPro.Models
{
    public class EdProContext:DbContext
    {

        public virtual DbSet<Competence> Competences { get; set; } = null!;
        public virtual DbSet<CompetencesType> CompetencesTypes { get; set; } = null!;
        public virtual DbSet<ControlType> ControlTypes { get; set; } = null!;
        public virtual DbSet<EdProgramType> EdProgramTypes { get; set; } = null!;
        public virtual DbSet<EducationProgram> EducationPrograms { get; set; } = null!;
        public virtual DbSet<EpSubjectCompetence> EpSubjectCompetences { get; set; } = null!;
        public virtual DbSet<EpSubjectLoutcome> EpSubjectLoutcomes { get; set; } = null!;
        public virtual DbSet<Faculty> Faculties { get; set; } = null!;
        public virtual DbSet<LearningOutcome> LearningOutcomes { get; set; } = null!;
        public virtual DbSet<Speciality> Specialities { get; set; } = null!;
        public virtual DbSet<SpecialityCompetence> SpecialityCompetences { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<University> Universities { get; set; } = null!;
        public EdProContext(DbContextOptions<EdProContext> options):base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Competence>(entity =>
            {
                entity.Property(e => e.Competence1)
                    .HasColumnType("text")
                    .HasColumnName("Competence");

                entity.HasOne(d => d.CompetenceType)
                    .WithMany(p => p.Competences)
                    .HasForeignKey(d => d.CompetenceTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Competences_CompetencesTypes");
            });

            modelBuilder.Entity<CompetencesType>(entity =>
            {
                entity.Property(e => e.CompType).HasMaxLength(60);
            });

            modelBuilder.Entity<ControlType>(entity =>
            {
                entity.Property(e => e.ControlTypeName).HasMaxLength(70);
            });

            modelBuilder.Entity<EdProgramType>(entity =>
            {
                entity.Property(e => e.TypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<EducationProgram>(entity =>
            {
                entity.Property(e => e.Edbo)
                    .HasMaxLength(20)
                    .HasColumnName("EDBO")
                    .IsFixedLength();

                entity.Property(e => e.ImplementationDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.EdPrType)
                    .WithMany(p => p.EducationPrograms)
                    .HasForeignKey(d => d.EdPrTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EducationPrograms_EdProgramTypes");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.EducationPrograms)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EducationPrograms_Faculties");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.EducationPrograms)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EducationPrograms_Specialities");
            });

            modelBuilder.Entity<EpSubjectCompetence>(entity =>
            {
                entity.HasOne(d => d.SpecialityCompetence)
                    .WithMany(p => p.EpSubjectCompetences)
                    .HasForeignKey(d => d.SpecialityCompetenceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EpSubjectCompetences_SpecialityCompetences");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.EpSubjectCompetences)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EpSubjectCompetences_Subjects");
            });

            modelBuilder.Entity<EpSubjectLoutcome>(entity =>
            {
                entity.ToTable("EpSubjectLOutcomes");

                entity.HasOne(d => d.LearningOutcome)
                    .WithMany(p => p.EpSubjectLoutcomes)
                    .HasForeignKey(d => d.LearningOutcomeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EpSubjectLOutcomes_LearningOutcomes");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.EpSubjectLoutcomes)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EpSubjectLOutcomes_Subjects");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Faculties)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Faculties_Universities");
            });

            modelBuilder.Entity<LearningOutcome>(entity =>
            {
                entity.Property(e => e.LearningOutcome1)
                    .HasColumnType("text")
                    .HasColumnName("LearningOutcome");

                entity.Property(e => e.Loname)
                    .HasMaxLength(50)
                    .HasColumnName("LOname");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.LearningOutcomes)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_LearningOutcomes_Specialities");
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SpecialityCompetence>(entity =>
            {
                entity.HasOne(d => d.Competence)
                    .WithMany(p => p.SpecialityCompetences)
                    .HasForeignKey(d => d.CompetenceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SpecialityCompetences_Competences");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.SpecialityCompetences)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SpecialityCompetences_Specialities");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.EprogramId).HasColumnName("EProgramId");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.Control)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.ControlId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Subjects_ControlTypes");

                entity.HasOne(d => d.Eprogram)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.EprogramId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Subjects_EducationPrograms");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.Property(e => e.Edbo)
                    .HasMaxLength(20)
                    .HasColumnName("EDBO")
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(150);
            });
        }
    }
}

