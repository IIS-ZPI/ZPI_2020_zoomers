using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;

namespace zpi_aspnet_test.Tests.Controllers
{
	[TestClass]
	public class CategorySelectionControllerTests
	{
		private ICategoryDatabaseAccess _categoryRepository;
		private IStateDatabaseAccess _stateRepository;
		private IProductDatabaseAccess _productRepository;

		[TestInitialize]
		public void Setup()
		{
			_stateRepository = Substitute.For<IStateDatabaseAccess>();
			_productRepository = Substitute.For<IProductDatabaseAccess>();
			_categoryRepository = Substitute.For<ICategoryDatabaseAccess>();
		}

		[TestMethod]
		public void TestMethod1()
		{
			//
			// TODO: Add test logic here
			//
		}
	}
}