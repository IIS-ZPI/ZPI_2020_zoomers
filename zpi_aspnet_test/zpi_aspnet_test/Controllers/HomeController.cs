using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.DataBaseUtilities;
using zpi_aspnet_test.DataBaseUtilities.DAOs;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DatabaseContextProvider.Instance.ConnectToDb("zoomers_sql_server");
            var productDatabase = new StandardProductDatabaseAccessor();
            var categoryDatabase = new StandardCategoryDatabaseAccessor();
            var stateDatabase = new StandardStateDatabaseAccessor();

            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.ProductSelectList = new SelectList(productDatabase.GetProducts(), "Name", "Name");
            mainViewModel.CategorySelectList = new SelectList(categoryDatabase.GetCategories(), "Name", "Name");
            mainViewModel.StateSelectList = new SelectList(stateDatabase.GetStates(), "Name", "Name");

            DatabaseContextProvider.Instance.DisconnectFromDb();

            return View(mainViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "About this app";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "We are zoomers";

            return View();
        }
    }
}