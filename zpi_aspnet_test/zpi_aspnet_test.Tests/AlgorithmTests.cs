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
		private ProductModel _invalidProduct;
		private const int InvalidId = -1;
		[TestInitialize]
		public void Setup()
		{
			_state = new StateOfAmericaModel();
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
			Assert.That(() => Algorithm.CalculateFinalPrice(null, _state), Throws.An<NullReferenceException>());
		}

		[TestMethod]
		public void CalculateFinalPriceShouldThrowNullReferenceExceptionIfProvidedStateIsNull()
		{
			Assert.That(() => Algorithm.CalculateFinalPrice(_product, null), Throws.An<NullReferenceException>());
		}

		[TestMethod]
		public void GetTaxShouldThrowArgumentOutOfRangeExceptionIfProvidedCategoryIdIsIncorrect()
		{
			Assert.That(() => Algorithm.CalculateFinalPrice(_invalidProduct, null), Throws.An<ArgumentOutOfRangeException>());
		}

	}
}
