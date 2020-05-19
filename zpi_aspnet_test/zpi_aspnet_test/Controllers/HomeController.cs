using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.DataBaseUtilities;
using zpi_aspnet_test.DataBaseUtilities.DAOs;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DatabaseContextProvider.Instance.ConnectToDb("zoomers_sql_server");
            StandardProductDatabaseAccessor productDatabase = new StandardProductDatabaseAccessor();

            ProductModel productModel = new ProductModel();
            productModel.ProductList = new SelectList(productDatabase.GetProducts(), "id", "Name");

            return View(productModel);
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