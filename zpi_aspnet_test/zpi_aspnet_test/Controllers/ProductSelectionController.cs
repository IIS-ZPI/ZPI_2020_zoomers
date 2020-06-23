using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.ViewModels;
using static zpi_aspnet_test.Utilities.DoubleUtilities;

namespace zpi_aspnet_test.Controllers
{
	public class ProductSelectionController : Controller
	{
		private readonly IStateDatabaseAccess _stateDatabase;
		private readonly ICategoryDatabaseAccess _categoryDatabase;
		private readonly IProductDatabaseAccess _productDatabase;

		public ProductSelectionController(IStateDatabaseAccess stateDatabase, ICategoryDatabaseAccess categoryDatabase,
			IProductDatabaseAccess productDatabase)
		{
			_stateDatabase = stateDatabase;
			_categoryDatabase = categoryDatabase;
			_productDatabase = productDatabase;
		}

		[HttpPost]
		public ActionResult Index(string product, string preferredPriceInput, int count)
		{
			if (product == null || string.IsNullOrEmpty(product) || preferredPriceInput == null ||
				string.IsNullOrEmpty(preferredPriceInput) || count < 1)
				throw new HttpException(403, "The server cannot process request due to malformed or empty syntax");

			try
			{
				var states = _stateDatabase.GetStates();
				var products = _productDatabase.GetProducts();
				var categories = _categoryDatabase.GetCategories();

				var chosenProduct = products.FirstOrDefault(p => p.Name.Equals(product.Trim())) ??
									throw new HttpException(404, "Requested resource does not exist");

				chosenProduct.PreferredPrice = ParsePrice(preferredPriceInput);

				var stateNameList = new List<string>();
				var finalPrice = new List<double>();
				var tax = new List<double>();
				var margin = new List<double>();

				foreach (var state in states)
				{
					// State Name
					stateNameList.Add(state.Name);

					// Product final price in current state
					Algorithm.CalculateFinalPrice(chosenProduct, state, count);
					finalPrice.Add(chosenProduct.FinalPrice);

					// Tax for current state
					tax.Add(Algorithm.GetTax(chosenProduct, state, count));

					// Margin for chosen product in current state
					margin.Add(Algorithm.CalculateMargin(chosenProduct, count));
				}

				var mainViewModel = new MainViewModel
				{
					ProductSelectList = new SelectList(products, "Name", "Name"),
					CategorySelectList = new SelectList(categories, "Name", "Name"),
					StateSelectList = new SelectList(states, "Name", "Name"),
					PurchasePrice = Math.Round(chosenProduct.PurchasePrice, 2),
					PreferredPrice = chosenProduct.PreferredPrice,
					NumberOfProducts = count,
					Tax = tax,
					Margin = margin,
					FinalPrice = finalPrice,
					StateNameList = stateNameList,
					ChosenProduct = chosenProduct
				};

				return View(mainViewModel);
			}
			catch (HttpException)
			{
				throw;
			}
			catch (Exception)
			{
				throw new HttpException(500, "Server encountered the problem with access to demanded resources");
			}
		}
	}
}