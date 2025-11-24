using Microsoft.AspNetCore.Mvc;
using Firmness.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Firmness.Web.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Firmness.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ImportController : Controller
    {
        private readonly ExcelImportService _importService;

        public ImportController(ApplicationDbContext context)
        {
            _importService = new ExcelImportService(context);
        }

        public IActionResult Index()
        {
            return View("~/Views/Admin/Import/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return View("~/Views/Admin/Import/Index.cshtml");
            }

            var errorLog = await _importService.ImportData(file.OpenReadStream());

            ViewBag.ErrorLog = errorLog;
            return View("~/Views/Admin/Import/Results.cshtml");
        }
    }
}
