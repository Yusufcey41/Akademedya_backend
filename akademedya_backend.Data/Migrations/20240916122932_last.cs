using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace akademedya_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "table_informations",
                schema: "dbo",
                columns: table => new
                {
                    table_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    table_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    table_image = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_table_informations", x => x.table_id);
                });

            migrationBuilder.CreateTable(
                name: "user_informations",
                schema: "dbo",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_informations", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                schema: "dbo",
                columns: table => new
                {
                    table_id = table.Column<int>(type: "int", nullable: false),
                    column_id = table.Column<int>(type: "int", nullable: false),
                    column_name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => new { x.table_id, x.column_id });
                    table.ForeignKey(
                        name: "fk_columntable_id",
                        column: x => x.table_id,
                        principalSchema: "dbo",
                        principalTable: "table_informations",
                        principalColumn: "table_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TablesValues",
                schema: "dbo",
                columns: table => new
                {
                    table_id = table.Column<int>(type: "int", nullable: false),
                    input_area_id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "nvarchar(75)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablesValues", x => new { x.table_id, x.input_area_id });
                    table.ForeignKey(
                        name: "fk_valuetable_id",
                        column: x => x.table_id,
                        principalSchema: "dbo",
                        principalTable: "table_informations",
                        principalColumn: "table_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTables",
                schema: "dbo",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    table_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTables", x => new { x.table_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_table_id",
                        column: x => x.table_id,
                        principalSchema: "dbo",
                        principalTable: "table_informations",
                        principalColumn: "table_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "user_informations",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTables_table_id",
                schema: "dbo",
                table: "UserTables",
                column: "table_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTables_user_id",
                schema: "dbo",
                table: "UserTables",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Columns",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TablesValues",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserTables",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "table_informations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "user_informations",
                schema: "dbo");
        }
    }
}
