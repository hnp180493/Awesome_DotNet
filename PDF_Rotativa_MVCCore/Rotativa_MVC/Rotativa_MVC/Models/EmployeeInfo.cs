using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rotativa_MVC.Models
{
    public class EmployeeInfo
    {
        [Key]
        public int EmpNo { get; set; }
        public string EmpName { get; set; }
        public int Salary { get; set; }
        public string DeptName { get; set; }
        public string Designation { get; set; }
        public decimal HRA { get; set; }
        public decimal TA { get; set; }
        public decimal DA { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TDS { get; set; }
        public decimal NetSalary { get; set; }
    }
}
