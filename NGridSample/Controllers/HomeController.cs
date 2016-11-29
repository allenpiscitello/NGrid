using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NGridSample.Controllers
{
    using Shared;
    using Microsoft.AspNetCore.Mvc;


    public class HomeController : Controller
    {
        private readonly ApiContext _context;

        public HomeController(ApiContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetData()
        {
            var data = _context.Items;

            var columns = new object[] {new {Name= "Column1"}, new {Name="Column2"}};
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
