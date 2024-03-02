using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAC.Data.Migrations;

/// <inheritdoc />
public partial class IntialData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ToDoList",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ToDoList", x => x.Id);
                table.ForeignKey(
                    name: "FK_ToDoList_AspNetUsers_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ToDoItem",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ToDoListId = table.Column<int>(type: "int", nullable: false),
                IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                DueBy = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ToDoItem", x => x.Id);
                table.ForeignKey(
                    name: "FK_ToDoItem_ToDoList_ToDoListId",
                    column: x => x.ToDoListId,
                    principalTable: "ToDoList",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ToDoItem_ToDoListId",
            table: "ToDoItem",
            column: "ToDoListId");

        migrationBuilder.CreateIndex(
            name: "IX_ToDoList_OwnerId",
            table: "ToDoList",
            column: "OwnerId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ToDoItem");

        migrationBuilder.DropTable(
            name: "ToDoList");
    }
}
