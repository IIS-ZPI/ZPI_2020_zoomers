using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zpi_aspnet_test;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.DAOs;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;

namespace zpi_aspnet_test.Tests.Controllers
{
	 [TestClass]
	 public class HomeControllerTest
	 {
		 private ICategoryDatabaseAccess _categoryRepository;
		 private IStateDatabaseAccess _stateRepository;
		 private IProductDatabaseAccess _productRepository;

		 [TestInitialize]
		 public void Setup()
		 {
			 _stateRepository = new StandardStateDatabaseAccessor();
			 _productRepository = new StandardProductDatabaseAccessor();
			 _categoryRepository = new StandardCategoryDatabaseAccessor();
		 }

		  [TestMethod]
		  public void Index()
		  {
				// Arrange
				HomeController controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

				// Act
				ViewResult result = controller.Index() as ViewResult;

				// Assert
				Assert.IsNotNull(result);
		  }

		  [TestMethod]
		  public void About()
		  {
				// Arrange
				HomeController controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

				// Act
				ViewResult result = controller.About() as ViewResult;

				// Assert
				Assert.AreEqual("About this app", result.ViewBag.Message);
		  }

		  [TestMethod]
		  public void Contact()
		  {
				// Arrange
				HomeController controller = new HomeController(_categoryRepository, _stateRepository, _productRepository);

				// Act
				ViewResult result = controller.Contact() as ViewResult;

				// Assert
				Assert.IsNotNull(result);
		  }
	 }
}
