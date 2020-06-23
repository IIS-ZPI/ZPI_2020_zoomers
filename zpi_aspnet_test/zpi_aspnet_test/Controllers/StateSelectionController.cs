using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.ViewModels;
using static zpi_aspnet_test.Utilities.DoubleUtilities;

namespace zpi_aspnet_test.Controllers
{
	public class StateSelectionController : Controller
	{
		private readonly IStateDatabaseAccess _stateDatabase;
		private readonly ICategoryDatabaseAccess _categoryDatabase;
		private readonly IProductDatabaseAccess _productDatabase;

		public StateSelectionController(IStateDatabaseAccess stateDatabase, ICategoryDatabaseAccess categoryDatabase,
			IProductDatabaseAccess productDatabase)
		{
			_stateDatabase = stateDatabase;
			_categoryDatabase = categoryDatabase;
			_productDatabase = productDatabase;
		}

		// GET: StateSelection
		public ActionResult Index(string product, string state, string preferredPriceInput = "0", int count = 1)
		{
			if (string.IsNullOrEmpty(product) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(preferredPriceInput) || count < 1)
				throw new HttpException(403, "The server cannot process request due to malformed or empty syntax");

			var productModels = _productDatabase.GetProducts();
			var categoryModels = _categoryDatabase.GetCategories();
			var stateOfAmericaModels = _stateDatabase.GetStates();

			var chosenState = _stateDatabase.GetStateByName(state.Trim());
			var chosenProduct = _productDatabase.GetProductByName(product.Trim());

			chosenProduct.PreferredPrice = ParsePrice(preferredPriceInput);

			// State Name
			var stateNameList = new List<string> {chosenState.Name};

			// Product final price in current state
			Algorithm.CalculateFinalPrice(chosenProduct, chosenState, count);
			var finalPriceList = new List<double> {chosenProduct.FinalPrice};

			// Tax for current state
			var taxes = new List<double> {Algorithm.GetTax(chosenProduct, chosenState, count)};

			// Margin for chosen product in current state
			var margins = new List<double> {Algorithm.CalculateMargin(chosenProduct, count)};

			var mainViewModel = new MainViewModel
			{
				ProductSelectList = new SelectList(productModels, "Name", "Name"),
				CategorySelectList = new SelectList(categoryModels, "Name", "Name"),
				StateSelectList = new SelectList(stateOfAmericaModels, "Name", "Name"),
				Tax = taxes,
				Margin = margins,
				FinalPrice = finalPriceList,
				StateNameList = stateNameList,
				ChosenProduct = chosenProduct,
				PurchasePrice = Math.Round(chosenProduct.PurchasePrice, 2),
				PreferredPrice = chosenProduct.PreferredPrice,
				NumberOfProducts = count,
				ChosenState = chosenState
			};

			return View(mainViewModel);
		}
	}
}