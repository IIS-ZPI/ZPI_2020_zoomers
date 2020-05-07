using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ProductDBHandle handle = new ProductDBHandle();
            ProductModel product = new ProductModel();
            
            product.ProductList = new SelectList(handle.GetProducts(), "id", "Name");
            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}