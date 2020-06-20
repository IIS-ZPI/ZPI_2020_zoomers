using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest;
using NHamcrest.Core;

using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Models;

using Assert = NHamcrest.XUnit.Assert;

using static zpi_aspnet_test.Tests.Builders.ProductBuilder;
using static zpi_aspnet_test.Tests.Builders.StateBuilder;

namespace zpi_aspnet_test.Tests
{
	/// <summary>
	/// Summary description for AlgorithmTests
	/// </summary>
	[TestClass]
	public class AlgorithmTests
	{
		private StateOfAmericaModel _state;
		private ProductModel _product;
		private const double PreferredPrice = 12.0;
		private const double PreferredPriceOf40 = 40.0;
		private const double FinalPriceOf36 = 36.0;
		private ProductModel _invalidProduct;
		private const int InvalidId = -1;

		[TestInitialize]
		public void Setup()
		{
			_state = State().AddTax().OfCategoryId((int)ProductCategoryEnum.Clothing).ConfirmTax().Build();

			_product = Product().WithCategoryId((int) ProductCategoryEnum.Clothing).Build();

			_invalidProduct = Product().WithCategoryId(InvalidId).Build();

		}

		[TestMethod]
		public void CalculateFinalPriceShouldThrowNullReferenceExceptionIfProvidedProductIsNull()
		{
			void CallingCalculateFinalPriceWithPassedNullProductModel() => Algorithm.CalculateFinalPrice(null, _state, 1);

			Assert.That(CallingCalculateFinalPriceWithPassedNullProductModel, Throws.An<NullReferenceException>());
		}

		[TestMethod]
		public void CalculateFinalPriceShouldThrowNullReferenceExceptionIfProvidedStateIsNull()
		{
			void CallingCalculateFinalPriceWithPassedNullStateModel() => Algorithm.CalculateFinalPrice(_product, null, 1);

			Assert.That(CallingCalculateFinalPriceWithPassedNullStateModel, Throws.An<NullReferenceException>());
		}

		[TestMethod]
		public void GetTaxShouldThrowArgumentOutOfRangeExceptionIfProvidedCategoryIdIsIncorrect()
		{
			void CallingGetTaxMethodWithPassedProductInstanceHavingIncorrectCategoryId() => Algorithm.CalculateFinalPrice(_invalidProduct, null, 1);

			Assert.That(CallingGetTaxMethodWithPassedProductInstanceHavingIncorrectCategoryId, Throws.An<ArgumentOutOfRangeException>());
		}

		[TestMethod]
		public void ApplyingTaxThatValueEqualsZeroShouldResultInFinalPriceThatIsEqualToPreferredPriceOfProductAfterThatCalculateFinalPriceIsCalled()
		{
			var productWithPreferredPrice = Product()
				.WithCategoryId((int)ProductCategoryEnum.Clothing).WithPreferredPrice(PreferredPrice)
				.Build();

			void StandardCalculateFinalPriceCall() => Algorithm.CalculateFinalPrice(productWithPreferredPrice, _state, 1);

			Assert.That(StandardCalculateFinalPriceCall,DoesNotThrow.An<ArgumentOutOfRangeException>());
			var finalPrice = productWithPreferredPrice.FinalPrice;
			Assert.That(finalPrice, Is.EqualTo(PreferredPrice));
		}

		[TestMethod]
		public void IfProductFinalPriceIsLesserThanPurchasePriceCalculateMarginShouldReturnNegativeValue()
		{
			var productWithFinalPriceLesserThanPurchasePrice = Product()
				.WithCategoryId((int) ProductCategoryEnum.Groceries).WithPurchasePrice(2137).WithFinalPrice(1488)
				.Build();

			var margin = Algorithm.CalculateMargin(productWithFinalPriceLesserThanPurchasePrice);

			Assert.That(margin, Is.LessThan(0.0));
		}

		[TestMethod]
		public void CalculateFinalPriceShouldUpdateProperlyFinalPriceOfProduct()
		{
			var state = State().AddTax().OfCategoryId((int)ProductCategoryEnum.Clothing).OfTaxRate(10).ConfirmTax().Build();
			var product = Product().WithCategoryId((int) ProductCategoryEnum.Clothing).WithPurchasePrice(20)
				.WithPreferredPrice(PreferredPriceOf40).Build();
			
			void StandardCalculateFinalPriceCall() => Algorithm.CalculateFinalPrice(product, state, 1);
			Assert.That(StandardCalculateFinalPriceCall, DoesNotThrow.An<ArgumentOutOfRangeException>());

			var finalPrice = product.FinalPrice;
			Assert.That(finalPrice, Is.EqualTo(FinalPriceOf36));
		}
	}
}
