using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FStudentManagement.Migrations
{
 /// <inheritdoc />
 public partial class Add_User_Admin_To_Roles : Migration
 {
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
   migrationBuilder.Sql("INsert into [dbo].[AspNetUserRoles] (UserId,RoleId) select '0920e883-c603-46d8-b33a-1d861d34b504' ,Id from [dbo].[AspNetRoles]");


  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
   migrationBuilder.Sql("delete from [dbo].[AspNetUserRoles] where UserId='0920e883-c603-46d8-b33a-1d861d34b504'");
  }
 }
}
