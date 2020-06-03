using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.DataBaseUtilities;
using zpi_aspnet_test.DataBaseUtilities.DAOs;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class ProductSelectionController : Controller
    {
        [HttpPost]
        public ActionResult Index(string product, double preferredPriceInput, int count)
        {
            DatabaseContextProvider.Instance.ConnectToDb("zoomers_sql_server");
            var productDatabase = new StandardProductDatabaseAccessor();
            var stateDatabase = new StandardStateDatabaseAccessor();
            var categoryDatabase = new StandardCategoryDatabaseAccessor();

            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.ProductSelectList = new SelectList(productDatabase.GetProducts(), "Name", "Name");
            mainViewModel.CategorySelectList = new SelectList(categoryDatabase.GetCategories(), "Name", "Name");
            mainViewModel.StateSelectList = new SelectList(stateDatabase.GetStates(), "Name", "Name");

            List<StateOfAmericaModel> stateList = new List<StateOfAmericaModel>(stateDatabase.GetStates());

            ProductModel chosenProduct = productDatabase.GetProductByName(product.Trim());
            chosenProduct.PreferredPrice = preferredPriceInput;

            mainViewModel.PurchasePrice = Math.Round(chosenProduct.PurchasePrice, 2);
            mainViewModel.PreferredPrice = chosenProduct.PreferredPrice;
            mainViewModel.NumberOfProducts = count;

            var stateNameList = new List<string>();
            var finalPrice = new List<double>();
            var tax = new List<double>();
            var margin = new List<double>();
            

            foreach (var state in stateList)
            {
                // State Name
                stateNameList.Add(state.Name);

                // Product final price in current state
                Algorithm.CalculateFinalPrice(chosenProduct, state, mainViewModel.NumberOfProducts);
                finalPrice.Add(chosenProduct.FinalPrice);

                // Tax for current state
                tax.Add(Algorithm.GetTax(chosenProduct, state));

                // Margin for chosen product in current state
                margin.Add(Algorithm.CalculateMargin(chosenProduct));
            }

            mainViewModel.Tax = tax;
            mainViewModel.Margin = margin;
            mainViewModel.FinalPrice = finalPrice;
            mainViewModel.StateNameList = stateNameList;
            mainViewModel.ChosenProduct = chosenProduct;

            DatabaseContextProvider.Instance.DisconnectFromDb();

            return View(mainViewModel);
        }
    }
}