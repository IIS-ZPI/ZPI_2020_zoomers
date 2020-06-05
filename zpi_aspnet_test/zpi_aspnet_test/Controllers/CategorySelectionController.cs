using System.Collections.Generic;
using System.Web.Mvc;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Controllers
{
    public class CategorySelectionController : Controller
    {
	    private readonly ICategoryDatabaseAccess _categoryRepository;
	    private readonly IStateDatabaseAccess _stateRepository;
	    private readonly IProductDatabaseAccess _productRepository;

	    public CategorySelectionController(ICategoryDatabaseAccess categoryRepository, IStateDatabaseAccess stateRepository, IProductDatabaseAccess productRepository)
	    {
		    _categoryRepository = categoryRepository;
		    _stateRepository = stateRepository;
		    _productRepository = productRepository;
	    }

	    [HttpPost]
        public ActionResult Index(string category)
        {
            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.ProductSelectList = new SelectList(_productRepository.GetProducts(), "Name", "Name");
            mainViewModel.CategorySelectList = new SelectList(_categoryRepository.GetCategories(), "Name", "Name");
            mainViewModel.StateSelectList = new SelectList(_stateRepository.GetStates(), "Name", "Name");

            mainViewModel.ProductList = new List<ProductModel>(
                _productRepository.GetProductsFromCategory(_categoryRepository.GetCategoryByName(category)));
            mainViewModel.Category = category;

            return View(mainViewModel);
        }
    }
}