using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class ProductSelectionController : Controller
    {
	    private readonly IStateDatabaseAccess _stateDatabase;
	    private readonly ICategoryDatabaseAccess _categoryDatabase;
	    private readonly IProductDatabaseAccess _productDatabase;

	    public ProductSelectionController(IStateDatabaseAccess stateDatabase, ICategoryDatabaseAccess categoryDatabase, IProductDatabaseAccess productDatabase)
	    {
		    _stateDatabase = stateDatabase;
		    _categoryDatabase = categoryDatabase;
		    _productDatabase = productDatabase;
	    }

	    [HttpPost]
        public ActionResult Index(string product, string preferredPriceInput, int count)
        {
	        MainViewModel mainViewModel = new MainViewModel
	        {
		        ProductSelectList = new SelectList(_productDatabase.GetProducts(), "Name", "Name"),
		        CategorySelectList = new SelectList(_categoryDatabase.GetCategories(), "Name", "Name"),
		        StateSelectList = new SelectList(_stateDatabase.GetStates(), "Name", "Name")

	        };

	        List<StateOfAmericaModel> stateList = new List<StateOfAmericaModel>(_stateDatabase.GetStates());

            ProductModel chosenProduct = _productDatabase.GetProductByName(product.Trim());

            NumberFormatInfo format = new NumberFormatInfo();
            if (preferredPriceInput.Contains(".") && preferredPriceInput.Contains(","))
            {
                format.NumberGroupSeparator = ",";
                format.NumberDecimalSeparator = ".";
            }
            else if (preferredPriceInput.Contains("."))
            {
                format.NumberDecimalSeparator = ".";
            }
            else if (preferredPriceInput.Contains(","))
            {
                format.NumberDecimalSeparator = ",";
            }

            chosenProduct.PreferredPrice = Convert.ToDouble(preferredPriceInput, format);

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
                tax.Add(Algorithm.GetTax(chosenProduct, state, mainViewModel.NumberOfProducts));

                // Margin for chosen product in current state
                margin.Add(Algorithm.CalculateMargin(chosenProduct, mainViewModel.NumberOfProducts));
            }

            mainViewModel.Tax = tax;
            mainViewModel.Margin = margin;
            mainViewModel.FinalPrice = finalPrice;
            mainViewModel.StateNameList = stateNameList;
            mainViewModel.ChosenProduct = chosenProduct;

            return View(mainViewModel);
        }
    }
}