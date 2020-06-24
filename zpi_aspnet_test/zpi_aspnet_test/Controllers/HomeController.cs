using System;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.ViewModels;

namespace zpi_aspnet_test.Controllers
{
	public class HomeController : Controller
	{
		private readonly ICategoryDatabaseAccess _categoryRepository;
		private readonly IStateDatabaseAccess _stateRepository;
		private readonly IProductDatabaseAccess _productRepository;

		public HomeController(ICategoryDatabaseAccess categoryRepository, IStateDatabaseAccess stateRepository,
			IProductDatabaseAccess productRepository)
		{
			_categoryRepository = categoryRepository;
			_stateRepository = stateRepository;
			_productRepository = productRepository;
		}

		public ActionResult Index()
		{
			try
			{
				var mainViewModel = new MainViewModel
				{
					ProductSelectList = new SelectList(_productRepository.GetProducts(), "Name", "Name"),
					CategorySelectList = new SelectList(_categoryRepository.GetCategories(), "Name", "Name"),
					StateSelectList = new SelectList(_stateRepository.GetStates(), "Name", "Name")
				};

				return View(mainViewModel);
			}
			catch (AccessToNotConnectedDatabaseException)
			{
				throw new HttpException(500, "Server encountered the problem with access to data");
			}
			catch (Exception)
			{
				throw new HttpException(500, "Server encountered some problems, please contact support");
			}
		}

		public ActionResult About()
		{
			ViewBag.Message = "About this app";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "We are zoomers";

			return View();
		}
	}
}