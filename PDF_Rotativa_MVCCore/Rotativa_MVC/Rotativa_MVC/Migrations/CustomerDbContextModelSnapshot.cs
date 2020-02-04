﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rotativa_MVC.Models;

namespace Rotativa_MVC.Migrations
{
    [DbContext(typeof(CustomerDbContext))]
    partial class CustomerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Rotativa_MVC.Models.EmployeeInfo", b =>
                {
                    b.Property<int>("EmpNo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DA");

                    b.Property<string>("DeptName");

                    b.Property<string>("Designation");

                    b.Property<string>("EmpName");

                    b.Property<decimal>("GrossSalary");

                    b.Property<decimal>("HRA");

                    b.Property<decimal>("NetSalary");

                    b.Property<int>("Salary");

                    b.Property<decimal>("TA");

                    b.Property<decimal>("TDS");

                    b.HasKey("EmpNo");

                    b.ToTable("EmployeeInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
