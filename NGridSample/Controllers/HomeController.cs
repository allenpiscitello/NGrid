namespace NGridSample.Controllers
{
    using System.Threading.Tasks;
    using Features;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;


    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetData()
        {
            var result = await _mediator.SendAsync(new FetchDataQuery());
            return Json(result);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
