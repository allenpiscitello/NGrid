using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NGridSample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetData()
        {
            var columns = new object[] {new {Name= "Column1"}, new {Name="Column2"}};
            var data = new object[] {};
            return Json(new
            {
                Columns = columns,
                Data = data
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
