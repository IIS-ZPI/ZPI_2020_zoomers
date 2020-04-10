using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Models;
using zpi_aspnet_test_xpath_parser;

namespace zpi_aspnet_test.Tests.Feature_Parser.Tests
{
   [TestClass]
   public class ParserTestsScenario
   {

      private int CountOfStates;
      private List<ProductCategoryEnum> Categories;
      private List<double> AlaskaTaxRates;

      [TestInitialize]
      public void Setup()
      {
         AlaskaTaxRates = new List<double>{0,0,0,0,0,0};
         CountOfStates = 53;
         Categories = new List<ProductCategoryEnum>((ProductCategoryEnum[])Enum.GetValues(typeof(ProductCategoryEnum)));
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

         Assert.AreEqual(CountOfStates, collection.Count);
      }

      [TestMethod]
      public void TestGuamDoesntHaveSalesTaxRateForIntagibles()
      {
         var collection = Parser.GetStatesModelsFromWikipedia();
         var Guam = collection.First(model => model.Name.Equals("Guam"));

         Assert.IsNotNull(Guam);
         Assert.IsFalse(Guam.Rates.Keys.SequenceEqual(Categories));
      }

      [TestMethod]
      public void TestValuesOfAlaskaSalesTaxRates()
      {
         var collection = Parser.GetStatesModelsFromWikipedia();
         var Alaska = collection.First(model => model.Name.Equals("Alaska"));

         Assert.IsNotNull(Alaska);
         Assert.IsTrue(Alaska.Rates.Values.SequenceEqual(AlaskaTaxRates));
      }
   }
}
