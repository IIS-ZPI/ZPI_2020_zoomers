using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using Assert = NHamcrest.XUnit.Assert;
using static NHamcrest.Is;
using static zpi_aspnet_test.Tests.Matchers.DecoratorMatchers;
using static NHamcrest.Has;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace zpi_aspnet_test.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		private ICategoryDatabaseAccess _categoryRepository;
		private IStateDatabaseAccess _stateRepository;
		private IProductDatabaseAccess _productRepository;
		private const string ExpectedMessageOf500CodeForAccessTrouble = "Server encountered the problem with access to data";
		private const string ExpectedMessage = "About this app";
		private const int Http500 = 500;

		[TestInitialize]
		public void Setup()
		{
			_stateRepository = Substitute.For<IStateDatabaseAccess>();
			_productRepository = Substitute.For<IProductDatabaseAccess>();
			_categoryRepository = Substitute.For<ICategoryDatabaseAccess>();
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
			var exception = Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;

			Assert.That(code, Is(EqualTo(Http500)));
			Assert.That(message, Is(EqualTo(ExpectedMessageOf500CodeForAccessTrouble)));
		}
	}
}