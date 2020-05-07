using System.Collections.Generic;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	public class StandardCategoryDatabaseAccessor : ICategoryDatabaseAccess
	{
		public ICollection<CategoryModel> GetCategories()
		{
			throw new System.NotImplementedException();
		}

		public CategoryModel GetCategoryById(int id)
		{
			throw new System.NotImplementedException();
		}

		public CategoryModel GetCategoryByName(string name)
		{
			throw new System.NotImplementedException();
		}

		public void InsertCategory(string name)
		{
			throw new System.NotImplementedException();
		}

		public void InsertCategory(CategoryModel category)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateCategory(CategoryModel category)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateCategory(int id, string value)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteCategory(CategoryModel model)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteCategory(int categoryId)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteCategory(string name)
		{
			throw new System.NotImplementedException();
		}
	}
}