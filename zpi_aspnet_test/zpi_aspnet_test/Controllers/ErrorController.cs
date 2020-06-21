using System.Web.Mvc;
using zpi_aspnet_test.ViewModels;

namespace zpi_aspnet_test.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorViewModel ViewModel { get; set; }
        // GET: Error
        public ActionResult HandleError()
        {
            return View(ViewModel?? new ErrorViewModel{ErrorCode="500", ErrorMessage="Internal server error :("});
        }
    }
}