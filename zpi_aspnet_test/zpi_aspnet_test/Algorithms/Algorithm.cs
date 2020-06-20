using System;
using System.Linq;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Algorithms
{
	public static class Algorithm
	{
		public static void CalculateFinalPrice(ProductModel product, StateOfAmericaModel state, int numberOfProducts)
		{
			var tax = GetTax(product, state, numberOfProducts);

			product.FinalPrice = (Math.Round(product.PreferredPrice - (product.PreferredPrice * (tax / 100)), 2)) *
								 numberOfProducts;
		}

		public static double GetTax(ProductModel product, StateOfAmericaModel state, int count)
		{
			return state.TaxRates.Where(tax => tax.CategoryId == product.CategoryId)
					  .FirstOrDefault(model => model.IsMoneyInRange(product.PreferredPrice * count))?.TaxRate ??
				   throw new ArgumentOutOfRangeException();
		}

		public static double CalculateMargin(ProductModel product, int numberOfProducts = 1)
		{
			return Math.Round(product.FinalPrice - (product.PurchasePrice * numberOfProducts), 2);
		}

	}
}