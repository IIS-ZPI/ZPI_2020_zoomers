using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test_xpath_parser;
using Assert = NHamcrest.XUnit.Assert;
using static NHamcrest.Is;
using static NHamcrest.Has;
using static zpi_aspnet_test.Tests.Matchers.DecoratorMatchers;

namespace zpi_aspnet_test.Tests.Feature_Parser.Tests
{
	[TestClass]
	public class ParserTestsScenario
	{
		private const int ExpectedStatesWithExceptions = 3;
		private int _countOfStates;
		private List<double> _alaskaTaxRates;
		private const int Intangibles = (int) ProductCategoryEnum.Intangibles;

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
			Assert.That(collection, Is(NotNull()));
		}

		[TestMethod]
		public void TestCountOfStatesParsed()
		{
			var collection = Parser.GetStatesModelsFromWikipedia();

			Assert.That(_countOfStates, Is(EqualTo(collection.Count)));
		}

		[TestMethod]
		public void TestGuamDoesNotHaveSalesTaxRateForIntangibles()
		{
			var collection = Parser.GetStatesModelsFromWikipedia();
			var guam = collection.First(model => model.Name.Equals("Guam"));

			Assert.That(guam, Is(NotNull()));
			var categories = guam.TaxRates.Select(tax => tax.CategoryId).ToList();
			Assert.That(categories, Not(Has(Item(EqualTo(Intangibles)))));
		}

		[TestMethod]
		public void TestValuesOfAlaskaSalesTaxRates()
		{
			var collection = Parser.GetStatesModelsFromWikipedia();
			var alaska = collection.First(model => model.Name.Equals("Alaska"));

			Assert.That(alaska, Is(NotNull()));
			var rates = alaska.TaxRates.Select(model => model.TaxRate).ToList();
			Assert.That(rates, Is(CollectionEqualTo(_alaskaTaxRates)));
			
		}

		[TestMethod]
		public void ShouldExistsOnlyThreeStatesThatForCurrentlyAvailableTaxRatesObtainedFromParserHaveTaxExceptions()
		{
			var collection = Parser.GetStatesModelsFromWikipedia()
			   .Where(model => model.TaxRates.Any(tax => tax.MinValue > 0.0)).ToList();

			Assert.That(collection, Is(OfLength(ExpectedStatesWithExceptions)));
		}
	}
}