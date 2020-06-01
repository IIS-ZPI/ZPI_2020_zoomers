using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zpi_aspnet_test.Algorithms;
using zpi_aspnet_test.Models;

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
		}

		[TestMethod]
		public void CalculateFinalPriceShouldThrowNullReferenceExceptionIfProvidedProductIsNull()
		{
			Assert.ThrowsException<NullReferenceException>(() => Algorithm.CalculateFinalPrice(null, _state));
		}
	}
}
