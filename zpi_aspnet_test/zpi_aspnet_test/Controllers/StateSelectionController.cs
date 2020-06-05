using System;
using System.Collections.Generic;
using System.Web.Mvc;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class StateSelectionController : Controller
    {
	    private readonly IStateDatabaseAccess _stateDatabase;
	    private readonly ICategoryDatabaseAccess _categoryDatabase;
	    private readonly IProductDatabaseAccess _productDatabase;

	    public StateSelectionController(IStateDatabaseAccess stateDatabase, ICategoryDatabaseAccess categoryDatabase, IProductDatabaseAccess productDatabase)
	    {
		    _stateDatabase = stateDatabase;
		    _categoryDatabase = categoryDatabase;
		    _productDatabase = productDatabase;
	    }

	    // GET: StateSelection
        public ActionResult Index(string product, string state, double preferredPriceInput, int count)
        {
            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.ProductSelectList = new SelectList(_productDatabase.GetProducts(), "Name", "Name");
            mainViewModel.CategorySelectList = new SelectList(_categoryDatabase.GetCategories(), "Name", "Name");
            mainViewModel.StateSelectList = new SelectList(_stateDatabase.GetStates(), "Name", "Name");

            StateOfAmericaModel chosenState = _stateDatabase.GetStateByName(state.Trim());
            ProductModel chosenProduct = _productDatabase.GetProductByName(product.Trim());
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