using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class SchemeWasRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_TaskItems_TaskId",
                schema: "Tasks",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                schema: "Tasks",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskGroups_Projects_ProjectId",
                schema: "Tasks",
                table: "TaskGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_TaskGroups_GroupId",
                schema: "Tasks",
                table: "TaskItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskItems",
                schema: "Tasks",
                table: "TaskItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskGroups",
                schema: "Tasks",
                table: "TaskGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                schema: "Tasks",
                table: "ProjectMembers");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "Tasks",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "Tasks",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "TaskItems",
                schema: "Tasks",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "TaskGroups",
                schema: "Tasks",
                newName: "Groups");

            migrationBuilder.RenameTable(
                name: "ProjectMembers",
                schema: "Tasks",
                newName: "Members");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_ReporterId",
                table: "Tasks",
                newName: "IX_Tasks_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_GroupId_Priority",
                table: "Tasks",
                newName: "IX_Tasks_GroupId_Priority");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_GroupId",
                table: "Tasks",
                newName: "IX_Tasks_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_AssignedId",
                table: "Tasks",
                newName: "IX_Tasks_AssignedId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskGroups_ProjectId_Name",
                table: "Groups",
                newName: "IX_Groups_ProjectId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_TaskGroups_ProjectId",
                table: "Groups",
                newName: "IX_Groups_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_UserId",
                table: "Members",
                newName: "IX_Members_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_ProjectId_UserId",
                table: "Members",
                newName: "IX_Members_ProjectId_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "Members",
                newName: "IX_Members_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Projects_ProjectId",
                table: "Groups",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Projects_ProjectId",
                table: "Members",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Groups_GroupId",
                table: "Tasks",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Projects_ProjectId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Projects_ProjectId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Groups_GroupId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.EnsureSchema(
                name: "Tasks");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "Tasks");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comments",
                newSchema: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TaskItems",
                newSchema: "Tasks");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "ProjectMembers",
                newSchema: "Tasks");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "TaskGroups",
                newSchema: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ReporterId",
                schema: "Tasks",
                table: "TaskItems",
                newName: "IX_TaskItems_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_GroupId_Priority",
                schema: "Tasks",
                table: "TaskItems",
                newName: "IX_TaskItems_GroupId_Priority");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_GroupId",
                schema: "Tasks",
                table: "TaskItems",
                newName: "IX_TaskItems_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AssignedId",
                schema: "Tasks",
                table: "TaskItems",
                newName: "IX_TaskItems_AssignedId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_UserId",
                schema: "Tasks",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_ProjectId_UserId",
                schema: "Tasks",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_ProjectId_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_ProjectId",
                schema: "Tasks",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_ProjectId_Name",
                schema: "Tasks",
                table: "TaskGroups",
                newName: "IX_TaskGroups_ProjectId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_ProjectId",
                schema: "Tasks",
                table: "TaskGroups",
                newName: "IX_TaskGroups_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskItems",
                schema: "Tasks",
                table: "TaskItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                schema: "Tasks",
                table: "ProjectMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskGroups",
                schema: "Tasks",
                table: "TaskGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_TaskItems_TaskId",
                schema: "Tasks",
                table: "Comments",
                column: "TaskId",
                principalSchema: "Tasks",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                schema: "Tasks",
                table: "ProjectMembers",
                column: "ProjectId",
                principalSchema: "Tasks",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskGroups_Projects_ProjectId",
                schema: "Tasks",
                table: "TaskGroups",
                column: "ProjectId",
                principalSchema: "Tasks",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_TaskGroups_GroupId",
                schema: "Tasks",
                table: "TaskItems",
                column: "GroupId",
                principalSchema: "Tasks",
                principalTable: "TaskGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
