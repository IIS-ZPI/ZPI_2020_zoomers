using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.DataBaseUtilities;
using zpi_aspnet_test.DataBaseUtilities.DAOs;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class CategorySelectionController : Controller
    {

        [HttpPost]
        public ActionResult Index(string category)
        {
            var productDatabase = new StandardProductDatabaseAccessor();
            var categoryDatabase = new StandardCategoryDatabaseAccessor();
            var stateDatabase = new StandardStateDatabaseAccessor();

            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.ProductSelectList = new SelectList(productDatabase.GetProducts(), "Name", "Name");
            mainViewModel.CategorySelectList = new SelectList(categoryDatabase.GetCategories(), "Name", "Name");
            mainViewModel.StateSelectList = new SelectList(stateDatabase.GetStates(), "Name", "Name");

            mainViewModel.ProductList = new List<ProductModel>(
                productDatabase.GetProductsFromCategory(categoryDatabase.GetCategoryByName(category)));
            mainViewModel.Category = category;

            return View(mainViewModel);
        }
    }
}