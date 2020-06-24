using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;
using Assert = NHamcrest.XUnit.Assert;
using static NHamcrest.Is;
using static zpi_aspnet_test.Tests.Matchers.DecoratorMatchers;
using static NHamcrest.Has;

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

		private const string ExpectedMessageOf500CodeForAnyException =
			"Server encountered some problems, please contact support";

		private const string ExpectedMessageOf500CodeForAccessTrouble =
			"Server encountered the problem with access to data";

		private const string ExpectedMessage = "About this app";
		private const int Http500 = 500;

		[TestInitialize]
		public void Setup()
		{
			_stateRepository = Substitute.For<IStateDatabaseAccess>();
			_productRepository = Substitute.For<IProductDatabaseAccess>();
			_categoryRepository = Substitute.For<ICategoryDatabaseAccess>();

			_preparedStates = new List<StateOfAmericaModel>();
			_preparedCategories = new List<CategoryModel>();
			_preparedProducts = new List<ProductModel>();
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
		}
	}
}