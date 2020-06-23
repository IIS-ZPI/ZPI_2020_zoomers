using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
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
		private const string ExpectedMessage = "About this app";

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
   }
}