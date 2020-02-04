using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rotativa_MVC.Models
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<EmployeeInfo> EmployeeInfo { get; set; }
    }
}
