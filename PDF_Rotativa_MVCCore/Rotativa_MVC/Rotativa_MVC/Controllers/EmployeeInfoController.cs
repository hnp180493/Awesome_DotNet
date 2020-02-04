using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rotativa_MVC.Models;
using Rotativa.AspNetCore;

namespace Rotativa_MVC.Controllers
{
    //Report to pdf by html template with easily to use
    //1. Install Rotativa.AspNetCore via Nuget
    //2. Copy Rotaiva folder with files *.exe to wwwroot (this these files use to convert html to pdf)
    //3. Set up to get Rotaiva folder in Startup
    //4. In ActionResult using ViewAsPdf to export a html view to PDF
    public class EmployeeInfoController : Controller
    {
        CustomerDbContext _db;
        public EmployeeInfoController(CustomerDbContext db)
        {
            _db = db;
        }
        public ActionResult Index()
        {
            var emps = _db.EmployeeInfo.ToList();
            return View(emps);
        }
        public ActionResult PrintAllReport()
        {
            var emps = _db.EmployeeInfo.ToList();
            var report = new ViewAsPdf("Index", emps);
            return report;
        }
        public ActionResult IndexById(int id)
        {
            var emp = _db.EmployeeInfo.Where(e => e.EmpNo == id).First();
            return View(emp);
        }
        public ActionResult PrintSalarySlip(int id)
        {
            var emp = _db.EmployeeInfo.Where(e => e.EmpNo == id).First();
            var report = new ViewAsPdf("IndexById", emp);
            return report;
        }
    }
}