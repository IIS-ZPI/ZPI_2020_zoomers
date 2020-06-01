﻿using System;
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
		[TestInitialize]
		public void Setup()
		{
			_state = new StateOfAmericaModel();
			_product = new ProductModel
			{
				CategoryId = (int)ProductCategoryEnum.Clothing
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

	}
}