using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test_xpath_parser;

namespace zpi_aspnet_test.Tests.Feature_Parser.Tests
{
	[TestClass]
	public class ParserTestsScenario
	{
		private const double Epsilon = 0.00001;
		private int _countOfStates;
		private List<double> _alaskaTaxRates;

		[TestInitialize]
		public void Setup()
		{
			_alaskaTaxRates = new List<double> {0, 0, 0, 0, 0, 0};
			_countOfStates = 53;
		}

		[TestMethod]
		public void TestIfCollectionReturnedByParserIsNotNull()
		{
			var collection = Parser.GetStatesModelsFromWikipedia();
			Assert.IsNotNull(collection);
		}

		[TestMethod]
		public void TestCountOfStatesParsed()
		{
			var collection = Parser.GetStatesModelsFromWikipedia();

			Assert.AreEqual(_countOfStates, collection.Count);
		}

		[TestMethod]
		public void TestGuamDoesNotHaveSalesTaxRateForIntangibles()
		{
			var collection = Parser.GetStatesModelsFromWikipedia();
			var guam = collection.First(model => model.Name.Equals("Guam"));

			Assert.IsNotNull(guam);
			Assert.IsFalse(Math.Abs(guam.TaxRates
			   .FirstOrDefault(tax => tax.CategoryId == (int) ProductCategoryEnum.Intangibles)
			  ?.TaxRate ?? 0) > Epsilon);
		}

		[TestMethod]
		public void TestValuesOfAlaskaSalesTaxRates()
		{
			var collection = Parser.GetStatesModelsFromWikipedia();
			var alaska = collection.First(model => model.Name.Equals("Alaska"));

			Assert.IsNotNull(alaska);
			var rates = alaska.TaxRates.Select(model => model.TaxRate).ToList();
			Assert.IsTrue(rates.SequenceEqual(_alaskaTaxRates));
		}
	}
}