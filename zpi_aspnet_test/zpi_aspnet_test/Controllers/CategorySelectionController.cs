using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.ViewModels;

namespace zpi_aspnet_test.Controllers
{
	public class CategorySelectionController : Controller
	{
		private readonly ICategoryDatabaseAccess _categoryRepository;
		private readonly IStateDatabaseAccess _stateRepository;
		private readonly IProductDatabaseAccess _productRepository;

		public CategorySelectionController(ICategoryDatabaseAccess categoryRepository,
			IStateDatabaseAccess stateRepository, IProductDatabaseAccess productRepository)
		{
			_categoryRepository = categoryRepository;
			_stateRepository = stateRepository;
			_productRepository = productRepository;
		}

		[HttpPost]
		public ActionResult Index(string category)
		{
			if (category == null || string.IsNullOrEmpty(category))
				throw new HttpException(403, "The server cannot process request due to malformed or empty syntax");

			try
			{
				var productModels = _productRepository.GetProducts();
				var categoryModels = _categoryRepository.GetCategories();
				var stateOfAmericaModels = _stateRepository.GetStates();

				var mainViewModel = new MainViewModel
				{
					ProductSelectList = new SelectList(productModels, "Name", "Name"),
					CategorySelectList = new SelectList(categoryModels, "Name", "Name"),
					StateSelectList = new SelectList(stateOfAmericaModels, "Name", "Name"),
					ProductList =
						productModels.Where(product => product.Category.Name.Equals(category.Trim())).ToList(),
					Category = category
				};

				return View(mainViewModel);
			}
			catch (Exception)
			{
				throw new HttpException(500, "Server encountered the problem with access to demanded resources");
			}
		}
	}
}