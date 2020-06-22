using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.ViewModels;

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
			var productModels = _productDatabase.GetProducts();
			var categoryModels = _categoryDatabase.GetCategories();
			var stateOfAmericaModels = _stateDatabase.GetStates();

			if (state == null || product == null)
			{
				throw new HttpException(403, "The server cannot process request due to malformed or empty syntax");
			}

			var chosenState = _stateDatabase.GetStateByName(state.Trim());
			var chosenProduct = _productDatabase.GetProductByName(product.Trim());


			var format = new NumberFormatInfo();
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

			var stateNameList = new List<string>();
			var finalPrice = new List<double>();
			var tax = new List<double>();
			var margin = new List<double>();

			// State Name
			stateNameList.Add(chosenState.Name);

			// Product final price in current state
			Algorithm.CalculateFinalPrice(chosenProduct, chosenState, count);
			finalPrice.Add(chosenProduct.FinalPrice);

			// Tax for current state
			tax.Add(Algorithm.GetTax(chosenProduct, chosenState, count));

			// Margin for chosen product in current state
			margin.Add(Algorithm.CalculateMargin(chosenProduct, count));

			var mainViewModel = new MainViewModel
			{
				ProductSelectList = new SelectList(productModels, "Name", "Name"),
				CategorySelectList = new SelectList(categoryModels, "Name", "Name"),
				StateSelectList = new SelectList(stateOfAmericaModels, "Name", "Name"),
				Tax = tax,
				Margin = margin,
				FinalPrice = finalPrice,
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