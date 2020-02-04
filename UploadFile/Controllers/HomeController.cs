using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadFile.Models;

namespace UploadFile.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _db;
        public HomeController(ILogger<HomeController> logger, AppDbContext db, IWebHostEnvironment env)
        {
            _logger = logger;
            _db = db;
            _env = env;
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile fileUpload) //save as file Path
        {
            var rootPath = $"{_env.WebRootPath}/images";
            var fileName = Path.GetFileName(fileUpload.FileName);
            var filePath = Path.Combine(rootPath, fileName);

            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileUpload.CopyTo(fileStream);
            }

            var img = new Image
            {
                ImageTitle = fileName,
                ImagePath = $"/images/{fileName}"
            };

            _db.Images.Add(img);
            _db.SaveChanges();
            ViewBag.Message = "Image(s) stored in database!";
            return View("Index");
        }

        //[HttpPost]
        //public IActionResult UploadImage(IFormFile fileUpload) //save as Binary
        //{
        //    Image img = new Image();
        //    img.ImageTitle = fileUpload.FileName;

        //    MemoryStream ms = new MemoryStream();
        //    fileUpload.CopyTo(ms);
        //    img.ImageData = ms.ToArray();

        //    ms.Close();
        //    ms.Dispose();

        //    _db.Images.Add(img);
        //    _db.SaveChanges();
        //    ViewBag.Message = "Image(s) stored in database!";
        //    return View("Index");
        //}

        //[HttpPost]
        //public IActionResult UploadImage() //Get from request
        //{
        //    foreach (var file in Request.Form.Files)
        //    {
        //        Image img = new Image();
        //        img.ImageTitle = file.FileName;

        //        MemoryStream ms = new MemoryStream();
        //        file.CopyTo(ms);
        //        img.ImageData = ms.ToArray();

        //        ms.Close();
        //        ms.Dispose();

        //        _db.Images.Add(img);
        //        _db.SaveChanges();
        //    }
        //    ViewBag.Message = "Image(s) stored in database!";
        //    return View("Index");
        //}

        [HttpPost]
        public ActionResult RetrieveImage() // get image from path
        {
            Image img = _db.Images.OrderByDescending(i => i.Id).FirstOrDefault();
            ViewBag.ImageTitle = img.ImageTitle;
            ViewBag.ImageDataUrl = img.ImagePath;
            return View("Index");
        }

        //[HttpPost]
        //public ActionResult RetrieveImage() // convert image to base64
        //{
        //    Image img = _db.Images.OrderByDescending(i => i.Id).FirstOrDefault();
        //    string imageBase64Data = Convert.ToBase64String(img.ImageData);
        //    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
        //    ViewBag.ImageTitle = img.ImageTitle;
        //    ViewBag.ImageDataUrl = imageDataURL;
        //    return View("Index");
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
