using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;
using zpi_aspnet_test.ViewModels;
using static zpi_aspnet_test.Tests.Matchers.DecoratorMatchers;
using static NHamcrest.Is;
using static NHamcrest.Has;
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
	public class StateSelectionControllerTests
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
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			var result = controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			Assert.That(result, Is(NotNull()));
		}

		[TestMethod]
		public void IndexPageShouldCallGetStatesOfStatesRepositoryOnlyOneTime()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			_stateRepository.Received(1).GetStates();
		}

		[TestMethod]
		public void IndexPageShouldCallGetCategoriesOfCategoriesRepositoryOnlyOneTime()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);
			_categoryRepository.Received(1).GetCategories();
		}

		[TestMethod]
		public void IndexPageShouldCallGetProductsOfProductsRepositoryOnlyOneTime()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);
			_productRepository.Received(1).GetProducts();
		}

		[TestMethod]
		public void IndexShouldThrowHttpExceptionWithCode403AndSpecifiedMessageIfProductStringWillBeNullOrEmpty()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			ActionResult Call() => controller.Index(Empty, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			ActionResult CallWithNull() => controller.Index(null, StateName, ExampleCorrectPreferredPriceInput,
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

		[TestMethod]
		public void IndexShouldThrowHttpExceptionWithCode403AndSpecifiedMessageIfPreferredPriceStringWillBeNullOrEmpty()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			ActionResult Call() => controller.Index(ProductName, StateName, Empty,
				ExampleCorrectCount);

			ActionResult CallWithNull() => controller.Index(ProductName, StateName, null,
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

		[TestMethod]
		public void IndexShouldThrowHttpExceptionWithCode403AndSpecifiedMessageIfStateStringWillBeNullOrEmpty()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			ActionResult Call() => controller.Index(ProductName, Empty, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			ActionResult CallWithNull() => controller.Index(ProductName, null, ExampleCorrectPreferredPriceInput,
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

		[TestMethod]
		public void IndexShouldThrowHttpExceptionWithCode403AndSpecifiedMessageIfCountWillBeNegative()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			ActionResult Call() => controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleIncorrectCount);

			var exception = Xunit.Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;

			Assert.That(code, Is(EqualTo(Http403)));
			Assert.That(message, Is(EqualTo(ExpectedMessageFor403Code)));
		}

		[TestMethod]
		public void IndexShouldThrowHttpExceptionWithCode404AndSpecifiedMessageIfNoneProductWillBeAvailable()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Throws<ItemNotFoundException>();
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			ActionResult Call() => controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			var exception = Xunit.Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;
			Assert.That(code, Is(EqualTo(Http404)));
			Assert.That(message, Is(EqualTo(ExpectedMessageOf404Code)));
		}

		[TestMethod]
		public void
			IndexShouldThrowHttpExceptionWithCode404AndSpecifiedMessageIfStateDoesNotExistInDatabase()
		{
			_productRepository.GetProducts().Returns(_preparedProducts);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Throws<ItemNotFoundException>();
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			ActionResult Call() => controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			var exception = Xunit.Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;
			Assert.That(code, Is(EqualTo(Http404)));
			Assert.That(message, Is(EqualTo(ExpectedMessageOf404Code)));
		}

		[TestMethod]
		public void
			IndexShouldThrowHttpExceptionWithCode500AndSpecifiedMessageForAccessToDbIssuesIfAnyRepositoryThrewAnAccessToNotConnectedDatabaseException()
		{
			_stateRepository.GetStates().Throws(new AccessToNotConnectedDatabaseException());
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			ActionResult Call() => controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			var exception = Xunit.Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;

			Assert.That(code, Is(EqualTo(Http500)));
			Assert.That(message, Is(EqualTo(ExpectedMessageOf500CodeForAccessTrouble)));
		}

		[TestMethod]
		public void
			IndexShouldThrowHttpExceptionWithCode500AndSpecifiedMessageForAccessToDbIssuesIfAnyRepositoryThrewAnException()
		{
			_productRepository.GetProducts().Throws(new Exception());
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			ActionResult Call() => controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount);

			var exception = Xunit.Assert.Throws<HttpException>(Call);

			var code = exception.GetHttpCode();
			var message = exception.Message;

			Assert.That(code, Is(EqualTo(Http500)));
			Assert.That(message, Is(EqualTo(ExpectedMessageOf500CodeForAnyException)));
		}

		[TestMethod]
		public void IfIndexWillBeCorrectlyInvokedThenMainViewModelOfActionResultShouldMatchingToExpectedOne()
		{
			_stateRepository.GetStates().Returns(_preparedStates);
			_productRepository.GetProducts().Returns(_preparedProducts);
			_categoryRepository.GetCategories().Returns(_preparedCategories);
			_productRepository.GetProductByName(Arg.Any<string>()).Returns(_expectedProduct);
			_stateRepository.GetStateByName(Arg.Any<string>()).Returns(_expectedState);
			var controller = new StateSelectionController(_stateRepository, _categoryRepository, _productRepository);

			var result = controller.Index(ProductName, StateName, ExampleCorrectPreferredPriceInput,
				ExampleCorrectCount) as ViewResult;

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
			var taxes = viewModel?.Tax;
			Assert.That(taxes, Has(Item(EqualTo(_expectedState.BaseSalesTax))));
			var chosenProduct = viewModel?.ChosenProduct;
			Assert.That(chosenProduct, Is(EqualTo(_expectedProduct)));
			Assert.That(viewModel?.StateNameList, Has(Item(EqualTo(StateName))));
			Assert.That(viewModel?.PreferredPrice ?? 0, Is(EqualTo(PreferredPrice)));
			Assert.That(viewModel?.FinalPrice, Has(Item(EqualTo(PreferredPrice * ExampleCorrectCount))));
			
			const double diff = (PreferredPrice - PurchasePrice) * ExampleCorrectCount;
			Assert.That(viewModel?.Margin, Has(Item(EqualTo(diff))));

		}
	}
}