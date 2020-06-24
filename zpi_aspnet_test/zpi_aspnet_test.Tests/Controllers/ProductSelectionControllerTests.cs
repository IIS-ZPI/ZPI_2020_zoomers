using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;
using static zpi_aspnet_test.Tests.Matchers.DecoratorMatchers;
using static NHamcrest.Is;
using static zpi_aspnet_test.Tests.Builders.ProductBuilder;
using static zpi_aspnet_test.Tests.Builders.StateBuilder;
using static zpi_aspnet_test.Tests.Builders.TaxBuilder;
using static zpi_aspnet_test.Tests.Constants.StringConstants;
using static zpi_aspnet_test.Tests.Constants.IntConstants;
using static zpi_aspnet_test.Tests.Constants.DoubleConstants;
using Assert = NHamcrest.XUnit.Assert;

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
		private CategoryModel _expectedCategory;
		private StateOfAmericaModel _expectedState;
		private TaxModel _expectedTax;
		private ProductModel _expectedProduct;

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

		[TestMethod]
		public void IndexShouldThrowHttpExceptionWithCode403AndSpecifiedMessageIfProductStringWillBeNullOrEmpty()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			var controller = new ProductSelectionController(_stateRepository, _categoryRepository, _productRepository);

			ActionResult Call() => controller.Index(Empty, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			ActionResult CallWithNull() => controller.Index(null, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			var exception = Xunit.Assert.Throws<HttpException>(Call);
			var exception2 = Xunit.Assert.Throws<HttpException>(CallWithNull);

			var code = exception.GetHttpCode();
			var message = exception.Message;

			Assert.That(code, Is(EqualTo(Http403)));
			Assert.That(message, Is(EqualTo(ExpectedMessageFor403Code)));
			Assert.That(code, Is(EqualTo(exception2.GetHttpCode())));
			Assert.That(message, Is(EqualTo(exception2.Message)));
		}
	}
}