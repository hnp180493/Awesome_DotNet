using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rotativa_MVC.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeInfo",
                columns: table => new
                {
                    EmpNo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpName = table.Column<string>(nullable: true),
                    Salary = table.Column<int>(nullable: false),
                    DeptName = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    HRA = table.Column<decimal>(nullable: false),
                    TA = table.Column<decimal>(nullable: false),
                    DA = table.Column<decimal>(nullable: false),
                    GrossSalary = table.Column<decimal>(nullable: false),
                    TDS = table.Column<decimal>(nullable: false),
                    NetSalary = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInfo", x => x.EmpNo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeInfo");
        }
    }
}
