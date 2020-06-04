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
    public class StateSelectionController : Controller
    {
        // GET: StateSelection
        public ActionResult Index(string product, string state, double preferredPriceInput, int count)
        {
            var productDatabase = new StandardProductDatabaseAccessor();
            var categoryDatabase = new StandardCategoryDatabaseAccessor();
            var stateDatabase = new StandardStateDatabaseAccessor();

            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.ProductSelectList = new SelectList(productDatabase.GetProducts(), "Name", "Name");
            mainViewModel.CategorySelectList = new SelectList(categoryDatabase.GetCategories(), "Name", "Name");
            mainViewModel.StateSelectList = new SelectList(stateDatabase.GetStates(), "Name", "Name");

            StateOfAmericaModel chosenState = stateDatabase.GetStateByName(state.Trim());
            ProductModel chosenProduct = productDatabase.GetProductByName(product.Trim());
            chosenProduct.PreferredPrice = preferredPriceInput;

            mainViewModel.ChosenProduct = chosenProduct;
            mainViewModel.PurchasePrice = Math.Round(chosenProduct.PurchasePrice, 2);
            mainViewModel.PreferredPrice = chosenProduct.PreferredPrice;
            mainViewModel.NumberOfProducts = count;
            mainViewModel.ChosenState = chosenState;

            var stateNameList = new List<string>();
            var finalPrice = new List<double>();
            var tax = new List<double>();
            var margin = new List<double>();

            // State Name
            stateNameList.Add(chosenState.Name);

            // Product final price in current state
            Algorithm.CalculateFinalPrice(chosenProduct, chosenState, mainViewModel.NumberOfProducts);
            finalPrice.Add(chosenProduct.FinalPrice);

            // Tax for current state
            tax.Add(Algorithm.GetTax(chosenProduct, chosenState));

            // Margin for chosen product in current state
            margin.Add(Algorithm.CalculateMargin(chosenProduct));
            mainViewModel.Tax = tax;
            mainViewModel.Margin = margin;
            mainViewModel.FinalPrice = finalPrice;
            mainViewModel.StateNameList = stateNameList;
            mainViewModel.ChosenProduct = chosenProduct;
            return View(mainViewModel);
        }
    }
}