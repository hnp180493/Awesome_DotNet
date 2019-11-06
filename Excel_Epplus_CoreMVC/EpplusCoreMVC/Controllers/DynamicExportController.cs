using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EpplusCoreMVC.Manager;
using EpplusCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpplusCoreMVC.Controllers
{
    public class DynamicExportController : Controller
    {
        private readonly CustomerDbContext _db;
        public DynamicExportController(CustomerDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var propertyNames = new string[] {"CustomerName", "CustomerEmail"};
            var customers = _db.Customers.ToList();
            var stream = ExportManager.ExportExcelFile(customers, propertyNames, worksheetName: "Customer");

            var buffer = stream as MemoryStream;
            Response.Headers.Add("Content-Disposition", "attachment; filename=Customer.xlsx");
            Response.Body.WriteAsync(buffer.ToArray());

            return RedirectToAction("Index");
        }
    }
}