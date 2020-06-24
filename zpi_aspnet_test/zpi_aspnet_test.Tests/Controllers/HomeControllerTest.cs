using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;
using zpi_aspnet_test.ViewModels;
using Assert = NHamcrest.XUnit.Assert;
using static NHamcrest.Is;
using static zpi_aspnet_test.Tests.Matchers.DecoratorMatchers;
using static zpi_aspnet_test.Tests.Builders.ProductBuilder;
using static zpi_aspnet_test.Tests.Builders.StateBuilder;
using static zpi_aspnet_test.Tests.Builders.TaxBuilder;
using static NHamcrest.Has;
using static zpi_aspnet_test.Tests.Constants.StringConstants;
using static zpi_aspnet_test.Tests.Constants.IntConstants;
using static zpi_aspnet_test.Tests.Constants.DoubleConstants;
namespace zpi_aspnet_test.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
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
				Id = CategoryId, Name = CategoryName
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
		public void IndexPageShouldNotBeNullIfNoneErrorOccurred()
		{
			// Arrange
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

			// Act
			var result = controller.Index() as ViewResult;
			// Assert
			Assert.That(result, Is(NotNull()));
		}

		[TestMethod]
		public void AboutPageShouldNotBeNullAndShouldHaveSpecifiedMessageIfNoneErrorOccurred()
		{
			// Arrange
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

			// Act
			var result = controller.About() as ViewResult;

			// Assert
			Assert.That(result, Is(NotNull()));
			Assert.That(result?.ViewBag, Is(NotNull()));
			var message = result?.ViewBag?.Message;
			Assert.That(message, Is(EqualTo(ExpectedMessage)));
		}

		[TestMethod]
		public void ContactPageShouldNotBeNullIfNoneErrorOccurred()
		{
			// Arrange
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

			// Act
			var result = controller.Contact() as ViewResult;
			// Assert
			Assert.That(result, Is(NotNull()));
		}

		[TestMethod]
		public void IndexPageShouldCallGetStatesOfStatesRepositoryOnlyOneTime()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.Index();
			_stateRepository.Received(1).GetStates();
		}

		[TestMethod]
		public void IndexPageShouldCallGetCategoriesOfCategoriesRepositoryOnlyOneTime()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.Index();
			_categoryRepository.Received(1).GetCategories();
		}

		[TestMethod]
		public void IndexPageShouldCallGetProductsOfProductsRepositoryOnlyOneTime()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.Index();
			_productRepository.Received(1).GetProducts();
		}

		[TestMethod]
		public void IndexPageShouldThrowHttpExceptionWithStatusCodeEqualTo500AndSpecifiedMessageIfAnyRepositoryThrowErrorWithDbConnection()
		{
			_stateRepository.GetStates().Throws(new AccessToNotConnectedDatabaseException());

			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

			ActionResult Call() => controller.Index();
			var exception = Xunit.Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;

			Assert.That(code, Is(EqualTo(Http500)));
			Assert.That(message, Is(EqualTo(ExpectedMessageOf500CodeForAccessTrouble)));
		}

		[TestMethod]
		public void ContactPageShouldNotCallStatesRepository()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.Contact();
			_stateRepository.Received(0).GetStates();
			_stateRepository.Received(0).GetStateById(Arg.Any<int>());
			_stateRepository.Received(0).GetStateByName(Arg.Any<string>());
			_stateRepository.Received(0).UpdateState(Arg.Any<StateOfAmericaModel>());
			_stateRepository.Received(0).UpdateState(Arg.Any<int>());
			_stateRepository.Received(0).InsertState(Arg.Any<StateOfAmericaModel>());
			_stateRepository.Received(0).InsertState(Arg.Any<string>());
			_stateRepository.Received(0).DeleteState(Arg.Any<StateOfAmericaModel>());
			_stateRepository.Received(0).DeleteState(Arg.Any<int>());
			_stateRepository.Received(0).DeleteState(Arg.Any<string>());
		}

		[TestMethod]
		public void ContactPageShouldNotCallProductsRepository()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.Contact();
			_productRepository.Received(0).GetProducts();
			_productRepository.Received(0).GetProductById(Arg.Any<int>());
			_productRepository.Received(0).GetProductByName(Arg.Any<string>());
			_productRepository.Received(0).GetProductsFromCategory(Arg.Any<CategoryModel>());
			_productRepository.Received(0).InsertProduct(Arg.Any<ProductModel>());
			_productRepository.Received(0).InsertProduct(Arg.Any<string>(), Arg.Any<CategoryModel>());
			_productRepository.Received(0).InsertProduct(Arg.Any<string>(), Arg.Any<int>());
			_productRepository.Received(0).UpdateProduct(Arg.Any<ProductModel>());
			_productRepository.Received(0).UpdateProduct(Arg.Any<int>(), Arg.Any<int>());
			_productRepository.Received(0).UpdateProduct(Arg.Any<int>(), Arg.Any<CategoryModel>());
			_productRepository.Received(0).DeleteProduct(Arg.Any<ProductModel>());
			_productRepository.Received(0).DeleteProduct(Arg.Any<int>());
			_productRepository.Received(0).DeleteProduct(Arg.Any<string>());
		}

		[TestMethod]
		public void ContactPageShouldNotCallCategoriesRepository()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.Contact();
			_categoryRepository.Received(0).GetCategories();
			_categoryRepository.Received(0).GetCategoryById(Arg.Any<int>());
			_categoryRepository.Received(0).GetCategoryByName(Arg.Any<string>());
			_categoryRepository.Received(0).UpdateCategory(Arg.Any<CategoryModel>());
			_categoryRepository.Received(0).DeleteCategory(Arg.Any<CategoryModel>());
			_categoryRepository.Received(0).DeleteCategory(Arg.Any<int>());
			_categoryRepository.Received(0).DeleteCategory(Arg.Any<string>());
			_categoryRepository.Received(0).UpdateCategory(Arg.Any<int>(), Arg.Any<string>());
			_categoryRepository.Received(0).InsertCategory(Arg.Any<CategoryModel>());
			_categoryRepository.Received(0).InsertCategory(Arg.Any<string>());
		}

		[TestMethod]
		public void AboutPageShouldNotCallCategoriesRepository()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.About();
			_categoryRepository.Received(0).GetCategories();
			_categoryRepository.Received(0).GetCategoryById(Arg.Any<int>());
			_categoryRepository.Received(0).GetCategoryByName(Arg.Any<string>());
			_categoryRepository.Received(0).UpdateCategory(Arg.Any<CategoryModel>());
			_categoryRepository.Received(0).DeleteCategory(Arg.Any<CategoryModel>());
			_categoryRepository.Received(0).DeleteCategory(Arg.Any<int>());
			_categoryRepository.Received(0).DeleteCategory(Arg.Any<string>());
			_categoryRepository.Received(0).UpdateCategory(Arg.Any<int>(), Arg.Any<string>());
			_categoryRepository.Received(0).InsertCategory(Arg.Any<CategoryModel>());
			_categoryRepository.Received(0).InsertCategory(Arg.Any<string>());
		}

		[TestMethod]
		public void AboutPageShouldNotCallStatesRepository()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.About();
			_stateRepository.Received(0).GetStates();
			_stateRepository.Received(0).GetStateById(Arg.Any<int>());
			_stateRepository.Received(0).GetStateByName(Arg.Any<string>());
			_stateRepository.Received(0).UpdateState(Arg.Any<StateOfAmericaModel>());
			_stateRepository.Received(0).UpdateState(Arg.Any<int>());
			_stateRepository.Received(0).InsertState(Arg.Any<StateOfAmericaModel>());
			_stateRepository.Received(0).InsertState(Arg.Any<string>());
			_stateRepository.Received(0).DeleteState(Arg.Any<StateOfAmericaModel>());
			_stateRepository.Received(0).DeleteState(Arg.Any<int>());
			_stateRepository.Received(0).DeleteState(Arg.Any<string>());
		}

		[TestMethod]
		public void AboutPageShouldNotCallProductsRepository()
		{
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);
			controller.About();
			_productRepository.Received(0).GetProducts();
			_productRepository.Received(0).GetProductById(Arg.Any<int>());
			_productRepository.Received(0).GetProductByName(Arg.Any<string>());
			_productRepository.Received(0).GetProductsFromCategory(Arg.Any<CategoryModel>());
			_productRepository.Received(0).InsertProduct(Arg.Any<ProductModel>());
			_productRepository.Received(0).InsertProduct(Arg.Any<string>(), Arg.Any<CategoryModel>());
			_productRepository.Received(0).InsertProduct(Arg.Any<string>(), Arg.Any<int>());
			_productRepository.Received(0).UpdateProduct(Arg.Any<ProductModel>());
			_productRepository.Received(0).UpdateProduct(Arg.Any<int>(), Arg.Any<int>());
			_productRepository.Received(0).UpdateProduct(Arg.Any<int>(), Arg.Any<CategoryModel>());
			_productRepository.Received(0).DeleteProduct(Arg.Any<ProductModel>());
			_productRepository.Received(0).DeleteProduct(Arg.Any<int>());
			_productRepository.Received(0).DeleteProduct(Arg.Any<string>());
		}

		[TestMethod]
		public void IndexPageShouldThrowHttpExceptionWithStatusCodeEqualTo500AndSpecifiedMessageIfAnyRepositoryThrowUnspecifiedException()
		{
			_stateRepository.GetStates().Throws(new Exception());

			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

			ActionResult Call() => controller.Index();
			var exception = Xunit.Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;

			Assert.That(code, Is(EqualTo(Http500)));
			Assert.That(message, Is(EqualTo(ExpectedMessageOf500CodeForAnyException)));
		}

		[TestMethod]
		public void ViewModelOfIndexPageShouldHaveNotEmptySelectListsIfRepositoriesProvidedData()
		{
			_stateRepository.GetStates().Returns(_preparedStates);
			_productRepository.GetProducts().Returns(_preparedProducts);
			_categoryRepository.GetCategories().Returns(_preparedCategories);
			
			var controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

			var result = controller.Index() as ViewResult;
			// Assert
			Assert.That(result, Is(NotNull()));

			var viewModel = result?.Model as MainViewModel;

			Assert.That(viewModel, Is(NotNull()));

			var productsList = viewModel?.ProductSelectList.Items.Cast<ProductModel>().ToList();
			var statesList = viewModel?.StateSelectList.Items.Cast<StateOfAmericaModel>().ToList();
			var categoriesList = viewModel?.CategorySelectList.Items.Cast<CategoryModel>().ToList();

			Assert.That(productsList, Is<ICollection>(Not(OfLength(0))));
			Assert.That(statesList, Is<ICollection>(Not(OfLength(0))));
			Assert.That(categoriesList, Is<ICollection>(Not(OfLength(0))));
			
			Assert.That(productsList, Has(Item(EqualTo(_expectedProduct))));
			Assert.That(categoriesList, Has(Item(EqualTo(_expectedCategory))));
			Assert.That(statesList, Has(Item(EqualTo(_expectedState))));

			var state = statesList?.FirstOrDefault();

			Assert.That(state?.TaxRates, Has(Item((EqualTo(_expectedTax)))));
		}
	}
}