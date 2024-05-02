﻿// <auto-generated />
using System;
using FStudentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FStudentManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231224175344_Add_Roles")]
    partial class Add_Roles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FStudentManagement.Data.SUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("FStudentManagement.Models.ChildStage", b =>
                {
                    b.Property<int>("ChildStageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChildStageId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentStageId")
                        .HasColumnType("int");

                    b.HasKey("ChildStageId");

                    b.HasIndex("ParentStageId");

                    b.ToTable("childStages");
                });

            modelBuilder.Entity("FStudentManagement.Models.Course", b =>
                {
                    b.Property<int>("Course_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Course_ID"));

                    b.Property<int>("ChildStageId")
                        .HasColumnType("int");

                    b.Property<string>("Course_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentStageId")
                        .HasColumnType("int");

                    b.Property<int>("TermStageId")
                        .HasColumnType("int");

                    b.HasKey("Course_ID");

                    b.HasIndex("ChildStageId");

                    b.HasIndex("ParentStageId");

                    b.HasIndex("TermStageId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("FStudentManagement.Models.ParentStage", b =>
                {
                    b.Property<int>("ParentStageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParentStageId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParentStageId");

                    b.ToTable("parentStages");
                });

            modelBuilder.Entity("FStudentManagement.Models.Student", b =>
                {
                    b.Property<int>("Student_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Student_ID"));

                    b.Property<int>("ChildStageId")
                        .HasColumnType("int");

                    b.Property<int>("ParentStageId")
                        .HasColumnType("int");

                    b.Property<string>("Student_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TermStageId")
                        .HasColumnType("int");

                    b.HasKey("Student_ID");

                    b.HasIndex("ChildStageId");

                    b.HasIndex("ParentStageId");

                    b.HasIndex("TermStageId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("FStudentManagement.Models.StudentCourse", b =>
                {
                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.HasKey("StudentID", "CourseID");

                    b.HasIndex("CourseID");

                    b.ToTable("StudentCourses");
                });

            modelBuilder.Entity("FStudentManagement.Models.StudentTeacher", b =>
                {
                    b.Property<int>("Student_ID")
                        .HasColumnType("int");

                    b.Property<int>("Teacher_ID")
                        .HasColumnType("int");

                    b.HasKey("Student_ID", "Teacher_ID");

                    b.HasIndex("Teacher_ID");

                    b.ToTable("studentTeachers");
                });

            modelBuilder.Entity("FStudentManagement.Models.Teacher", b =>
                {
                    b.Property<int>("Teacher_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Teacher_ID"));

                    b.Property<int>("ChildStageId")
                        .HasColumnType("int");

                    b.Property<int>("ParentStageId")
                        .HasColumnType("int");

                    b.Property<string>("Teacher_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TermStageId")
                        .HasColumnType("int");

                    b.HasKey("Teacher_ID");

                    b.HasIndex("ChildStageId");

                    b.HasIndex("ParentStageId");

                    b.HasIndex("TermStageId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("FStudentManagement.Models.TeacherCourse", b =>
                {
                    b.Property<int>("Teacher_ID")
                        .HasColumnType("int");

                    b.Property<int>("Course_ID")
                        .HasColumnType("int");

                    b.HasKey("Teacher_ID", "Course_ID");

                    b.HasIndex("Course_ID");

                    b.ToTable("TeacherCourses");
                });

            modelBuilder.Entity("FStudentManagement.Models.TermStage", b =>
                {
                    b.Property<int>("TermStageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TermStageId"));

                    b.Property<int>("ChildStageId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TermStageId");

                    b.HasIndex("ChildStageId");

                    b.ToTable("TermStages");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FStudentManagement.Models.ChildStage", b =>
                {
                    b.HasOne("FStudentManagement.Models.ParentStage", "ParentStage")
                        .WithMany("ChildStages")
                        .HasForeignKey("ParentStageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentStage");
                });

            modelBuilder.Entity("FStudentManagement.Models.Course", b =>
                {
                    b.HasOne("FStudentManagement.Models.ChildStage", "ChildStage")
                        .WithMany("Courses")
                        .HasForeignKey("ChildStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.ParentStage", "ParentStage")
                        .WithMany("Courses")
                        .HasForeignKey("ParentStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.TermStage", "TermStage")
                        .WithMany("Courses")
                        .HasForeignKey("TermStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ChildStage");

                    b.Navigation("ParentStage");

                    b.Navigation("TermStage");
                });

            modelBuilder.Entity("FStudentManagement.Models.Student", b =>
                {
                    b.HasOne("FStudentManagement.Models.ChildStage", "ChildStage")
                        .WithMany("Students")
                        .HasForeignKey("ChildStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.ParentStage", "ParentStage")
                        .WithMany("Students")
                        .HasForeignKey("ParentStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.TermStage", "TermStage")
                        .WithMany("Students")
                        .HasForeignKey("TermStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ChildStage");

                    b.Navigation("ParentStage");

                    b.Navigation("TermStage");
                });

            modelBuilder.Entity("FStudentManagement.Models.StudentCourse", b =>
                {
                    b.HasOne("FStudentManagement.Models.Course", "Course")
                        .WithMany("StudentCourses")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.Student", "Student")
                        .WithMany("StudentCourses")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("FStudentManagement.Models.StudentTeacher", b =>
                {
                    b.HasOne("FStudentManagement.Models.Student", "Student")
                        .WithMany("StudentTeachers")
                        .HasForeignKey("Student_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.Teacher", "Teacher")
                        .WithMany("StudentTeachers")
                        .HasForeignKey("Teacher_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("FStudentManagement.Models.Teacher", b =>
                {
                    b.HasOne("FStudentManagement.Models.ChildStage", "ChildStage")
                        .WithMany("Teachers")
                        .HasForeignKey("ChildStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.ParentStage", "ParentStage")
                        .WithMany("Teachers")
                        .HasForeignKey("ParentStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.TermStage", "TermStage")
                        .WithMany("Teachers")
                        .HasForeignKey("TermStageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ChildStage");

                    b.Navigation("ParentStage");

                    b.Navigation("TermStage");
                });

            modelBuilder.Entity("FStudentManagement.Models.TeacherCourse", b =>
                {
                    b.HasOne("FStudentManagement.Models.Course", "Course")
                        .WithMany("TeacherCourses")
                        .HasForeignKey("Course_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Models.Teacher", "Teacher")
                        .WithMany("TeacherCourses")
                        .HasForeignKey("Teacher_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("FStudentManagement.Models.TermStage", b =>
                {
                    b.HasOne("FStudentManagement.Models.ChildStage", "ChildStage")
                        .WithMany("TermStages")
                        .HasForeignKey("ChildStageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChildStage");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FStudentManagement.Data.SUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FStudentManagement.Data.SUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FStudentManagement.Data.SUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FStudentManagement.Data.SUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FStudentManagement.Models.ChildStage", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Students");

                    b.Navigation("Teachers");

                    b.Navigation("TermStages");
                });

            modelBuilder.Entity("FStudentManagement.Models.Course", b =>
                {
                    b.Navigation("StudentCourses");

                    b.Navigation("TeacherCourses");
                });

            modelBuilder.Entity("FStudentManagement.Models.ParentStage", b =>
                {
                    b.Navigation("ChildStages");

                    b.Navigation("Courses");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("FStudentManagement.Models.Student", b =>
                {
                    b.Navigation("StudentCourses");

                    b.Navigation("StudentTeachers");
                });

            modelBuilder.Entity("FStudentManagement.Models.Teacher", b =>
                {
                    b.Navigation("StudentTeachers");

                    b.Navigation("TeacherCourses");
                });

            modelBuilder.Entity("FStudentManagement.Models.TermStage", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });
#pragma warning restore 612, 618
        }
    }
}
