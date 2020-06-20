using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult Index(string product, string state, string preferredPriceInput="0", int count=1)
        {
            
            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.ProductSelectList = new SelectList(_productDatabase.GetProducts(), "Name", "Name");
            mainViewModel.CategorySelectList = new SelectList(_categoryDatabase.GetCategories(), "Name", "Name");
            mainViewModel.StateSelectList = new SelectList(_stateDatabase.GetStates(), "Name", "Name");
            if (state == null || product == null)
            {
                mainViewModel.ChosenProduct = new ProductModel();
                mainViewModel.PurchasePrice = 0;
                mainViewModel.PreferredPrice = 0;
                mainViewModel.NumberOfProducts = 0;
                mainViewModel.ChosenState = new StateOfAmericaModel();

                mainViewModel.Tax = new List<double> { 6.9 };
                mainViewModel.Margin = new List<double> { 6.9 };
                mainViewModel.FinalPrice = new List<double> { 6.9 };
                mainViewModel.StateNameList = new List<string>();

                return View(mainViewModel);
            }

            StateOfAmericaModel chosenState = _stateDatabase.GetStateByName(state.Trim());
            ProductModel chosenProduct = _productDatabase.GetProductByName(product.Trim());


            NumberFormatInfo format = new NumberFormatInfo();
            if (preferredPriceInput.Contains(".") && preferredPriceInput.Contains(","))
            {
                format.NumberGroupSeparator = ",";
                format.NumberDecimalSeparator = ".";
            }else if (preferredPriceInput.Contains("."))
            {
                format.NumberDecimalSeparator = ".";
            }
            else if (preferredPriceInput.Contains(","))
            {
                format.NumberDecimalSeparator = ",";
            }



            chosenProduct.PreferredPrice = Convert.ToDouble(preferredPriceInput, format);

            mainViewModel.ChosenProduct = chosenProduct;//duplicate line ?
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
            tax.Add(Algorithm.GetTax(chosenProduct, chosenState, mainViewModel.NumberOfProducts));

            // Margin for chosen product in current state
            margin.Add(Algorithm.CalculateMargin(chosenProduct, mainViewModel.NumberOfProducts));
            mainViewModel.Tax = tax;
            mainViewModel.Margin = margin;
            mainViewModel.FinalPrice = finalPrice;
            mainViewModel.StateNameList = stateNameList;
            mainViewModel.ChosenProduct = chosenProduct;//duplicate line ?
            return View(mainViewModel);
        }
    }
}