using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;
using zpi_aspnet_test.Tests.Matchers;
using static zpi_aspnet_test.Tests.Matchers.DecoratorMatchers;
using static NHamcrest.Is;
using static NHamcrest.Has;
using Assert = NHamcrest.XUnit.Assert;
using static zpi_aspnet_test.Tests.Builders.ProductBuilder;
using static zpi_aspnet_test.Tests.Builders.StateBuilder;
using static zpi_aspnet_test.Tests.Builders.TaxBuilder;

namespace zpi_aspnet_test.Tests.Controllers
{
	[TestClass]
	public class ProductSelectionControllerTests
	{
		private ICategoryDatabaseAccess _categoryRepository;
		private IStateDatabaseAccess _stateRepository;
		private IProductDatabaseAccess _productRepository;

		private ICollection<CategoryModel> _preparedCategories;
		private ICollection<ProductModel> _preparedProducts;
		private ICollection<StateOfAmericaModel> _preparedStates;
		private const int ExampleCorrectCount = 2138;
		private const string ExampleCorrectPreferredPriceInput = "14.87";
		private CategoryModel _expectedCategory;
		private StateOfAmericaModel _expectedState;
		private TaxModel _expectedTax;
		private ProductModel _expectedProduct;

		private const string ExpectedMessageOf500CodeForAnyException =
			"Server encountered some problems, please contact support";

		private const string ExpectedMessageOf500CodeForAccessTrouble =
			"Server encountered the problem with access to data";

		private const string CategoryName = "TestCategory";
		private const string ProductName = "TestProduct";
		private const string StateName = "TestState";
		private const int CategoryId = 1;
		private const int ProductId = 1;
		private const int StateId = 1;
		private const int TaxId = 1;
		private const double TaxRate = 5.0;
		private const double MinMoney = 0.0;
		private const double MaxMoney = 250.0;
		private const double PurchasePrice = 21.36;
		private const string ExpectedMessage = "About this app";
		private const int Http500 = 500;

		[TestInitialize]
		public void Setup()
		{
			_stateRepository = Substitute.For<IStateDatabaseAccess>();
			_productRepository = Substitute.For<IProductDatabaseAccess>();
			_categoryRepository = Substitute.For<ICategoryDatabaseAccess>();
			_expectedCategory = new CategoryModel
			{
				Id = CategoryId,
				Name = CategoryName
			};

			_expectedProduct = Product().OfCategory(_expectedCategory).WithCategoryId(CategoryId)
			   .OfName(ProductName).WithId(ProductId).WithPurchasePrice(PurchasePrice).Build();

			_expectedTax = Tax().OfTaxRate(TaxRate).OfMinValue(MinMoney).OfMaxValue(MaxMoney)
			   .OfCategoryId(CategoryId).OfStateId(StateId).OfId(TaxId).Build();

			_expectedState = State().OfName(StateName).OfId(StateId)
			   .OfBaseSalesTax(TaxRate).AppendTax(_expectedTax).Build();

			_preparedStates = new List<StateOfAmericaModel> {_expectedState};
			_preparedCategories = new List<CategoryModel> {_expectedCategory};
			_preparedProducts = new List<ProductModel> {_expectedProduct};
		}

		[TestMethod]
		public void IndexShouldBeNotNullWhenProvidedArgumentsAreNotNullOrEmptyOrMalformed()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			var controller = new ProductSelectionController(_stateRepository, _categoryRepository, _productRepository);

			var result = controller.Index(ProductName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			Assert.That(result, Is(NotNull()));
		}

		[TestMethod]
		public void IndexPageShouldCallGetStatesOfStatesRepositoryOnlyOneTime()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			var controller = new ProductSelectionController(_stateRepository, _categoryRepository, _productRepository);
			controller.Index(ProductName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);
			_stateRepository.Received(1).GetStates();
		}

		[TestMethod]
		public void IndexPageShouldCallGetCategoriesOfCategoriesRepositoryOnlyOneTime()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			var controller = new ProductSelectionController(_stateRepository, _categoryRepository, _productRepository);
			controller.Index(ProductName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);
			_categoryRepository.Received(1).GetCategories();
		}

		[TestMethod]
		public void IndexPageShouldCallGetProductsOfProductsRepositoryOnlyOneTime()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			var controller = new ProductSelectionController(_stateRepository, _categoryRepository, _productRepository);
			controller.Index(ProductName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);
			_productRepository.Received(1).GetProducts();
		}
	}
}