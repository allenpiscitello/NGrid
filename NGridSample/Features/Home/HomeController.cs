namespace NGridSample.Features.Home
{
    using System.Threading.Tasks;
    using Domain;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using NGrid.Core;

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
        public async Task<JsonResult> GetData(FetchDataQuery<SampleItem, SampleItemViewModel> query)
        {
            var result = await _mediator.SendAsync(query);
            return Json(result);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
