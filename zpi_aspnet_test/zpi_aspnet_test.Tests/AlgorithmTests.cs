using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHamcrest.Core;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Models;
using Assert = NHamcrest.XUnit.Assert;
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
		private ProductModel _invalidProduct;
		private const int InvalidId = -1;

		[TestInitialize]
		public void Setup()
		{
			_state = new StateOfAmericaModel
			{
				Clothing = 0.0
			};

			_product = new ProductModel
			{
				CategoryId = (int)ProductCategoryEnum.Clothing
			};

			_invalidProduct = new ProductModel
			{
				CategoryId = InvalidId
			};


		}

		[TestMethod]
		public void CalculateFinalPriceShouldThrowNullReferenceExceptionIfProvidedProductIsNull()
		{
			void CallingCalculateFinalPriceWithPassedNullProductModel() => Algorithm.CalculateFinalPrice(null, _state);

			Assert.That(CallingCalculateFinalPriceWithPassedNullProductModel, Throws.An<NullReferenceException>());
		}

		[TestMethod]
		public void CalculateFinalPriceShouldThrowNullReferenceExceptionIfProvidedStateIsNull()
		{
			void CallingCalculateFinalPriceWithPassedNullStateModel() => Algorithm.CalculateFinalPrice(_product, null);

			Assert.That(CallingCalculateFinalPriceWithPassedNullStateModel, Throws.An<NullReferenceException>());
		}

		[TestMethod]
		public void GetTaxShouldThrowArgumentOutOfRangeExceptionIfProvidedCategoryIdIsIncorrect()
		{
			void CallingGetTaxMethodWithPassedProductInstanceHavingIncorrectCategoryId() => Algorithm.CalculateFinalPrice(_invalidProduct, null);

			Assert.That(CallingGetTaxMethodWithPassedProductInstanceHavingIncorrectCategoryId, Throws.An<ArgumentOutOfRangeException>());
		}

		[TestMethod]
		public void ApplyingTaxThatValueEqualsZeroShouldResultInFinalPriceThatIsEqualToPreferredPriceOfProductAfterThatCalculateFinalPriceIsCalled()
		{
			var productWithPreferredPrice = new ProductModel
			{
				CategoryId = (int)ProductCategoryEnum.Clothing,
				PreferredPrice = PreferredPrice
			};

			void StandardCalculateFinalPriceCall() => Algorithm.CalculateFinalPrice(productWithPreferredPrice, _state);

			Assert.That(StandardCalculateFinalPriceCall, Not(Throws.An<Exception>()));
		}
	}
}
