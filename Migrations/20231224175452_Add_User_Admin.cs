using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FStudentManagement.Migrations
{
 /// <inheritdoc />
 public partial class Add_User_Admin : Migration
 {
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
   migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'0920e883-c603-46d8-b33a-1d861d34b504', N'admin@admin.com', N'ADMIN@ADMIN.COM', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAELxa/FTFjemitVoNZdvhJHYXR1f5291rBAO5hLawqrr8+daVqJZULfkabLXt6EK63w==', N'G5ASX63E2SGLXXFKDJFVOD2M6J6C3WUQ', N'e15148b0-472d-4def-9ff2-3e33ce79efc1', NULL, 0, 0, NULL, 1, 0)");

  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
   migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id='0920e883-c603-46d8-b33a-1d861d34b504' ");

  }
 }
}
