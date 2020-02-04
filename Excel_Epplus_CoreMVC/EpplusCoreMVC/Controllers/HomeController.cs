using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EpplusCoreMVC.Models;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Table;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.Style;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;

namespace EpplusCoreMVC.Controllers
{
    //Install Epplus.Core in Nuget
    //No need set up in startup
    public class HomeController : Controller
    {
        private readonly CustomerDbContext _db;
        private readonly IHostingEnvironment _env;
        //private readonly HttpContext _context;
        public HomeController(CustomerDbContext db, IHostingEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Export()
        {
            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelFile();
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;

            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            Response.Headers.Add("Content-Disposition", "attachment; filename=ExcelDemo.xlsx");

            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.Body.WriteAsync(buffer.ToArray());

            // Send tất cả ouput bytes về phía clients
            //Response.Body.Flush();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 1. Initital ExcelPackage()
        /// 2. Create worksheet
        /// 3. Add header to worksheet
        /// 4. Using loop to fill data for each row
        /// 5. Save
        /// </summary>
        /// <returns></returns>
        public Stream CreateExcelFile()
        {
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Customer");
                workSheet.Cells[1, 1].Value = "Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Email";
                workSheet.Cells[1, 4].Value = "Country";

                // Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
                using (var range = workSheet.Cells["A1:D1"])
                {
                    // Set PatternType
                    range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                    // Set Màu cho Background
                    range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                    // Canh giữa cho các text
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Set Font cho text  trong Range hiện tại
                    range.Style.Font.SetFromFont(new Font("Arial", 10));
                    // Set Border
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    // Set màu ch Border
                    range.Style.Border.Bottom.Color.SetColor(Color.Blue);
                }

                var customers = _db.Customers.ToList();
                for (var i = 0; i < customers.Count; i++)
                {
                    int row = i + 2;
                    workSheet.Cells[row, 1].Value = customers[i].CustomerID;
                    workSheet.Cells[row, 2].Value = customers[i].CustomerName;
                    workSheet.Cells[row, 3].Value = customers[i].CustomerEmail;
                    workSheet.Cells[row, 4].Value = customers[i].CustomerCountry;
                }

                //workSheet.Cells[1, 1].LoadFromCollection(customers, true, TableStyles.Dark9);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                excel.Save();
                return excel.Stream;
            }
        }

        /// <summary>
        /// 1. Get info file via FileInfo()
        /// 2. Initital ExcelPackage() with file info parameter to work with this excel file
        /// 3. Get worksheet 
        /// 4. Loop these rows in worksheet and mapping its into object
        /// 5. Add object had already mapped into Database
        /// </summary>
        /// <returns></returns>
        public string ImportExcelFile()
        {
            var fileName = "ImportExcel.xlsx";
            var fileInfo = new FileInfo(Path.Combine(_env.WebRootPath, fileName));
            using (var excel = new ExcelPackage(fileInfo))
            {
                var workSheet = excel.Workbook.Worksheets[1];
                int totalRows = workSheet.Dimension.Rows;

                var lstCustomer = new List<Customer>();
                for (var i = 2; i <= totalRows; i++)
                {
                    var customer = new Customer
                    {
                        CustomerName = workSheet.Cells[i, 1].Value.ToString(),
                        CustomerEmail = workSheet.Cells[i, 2].Value.ToString(),
                        CustomerCountry = workSheet.Cells[i, 3].Value.ToString()
                    };

                    lstCustomer.Add(customer);
                }

                _db.Customers.AddRange(lstCustomer);
                _db.SaveChanges();
            }
            return "Success";
        }
    }
}
