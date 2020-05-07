using System.Collections.Generic;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.Interfaces
{
	public interface ICategoryDatabaseAccess
	{
		ICollection<CategoryModel> GetCategories();
		CategoryModel GetCategoryById(int id);
		CategoryModel GetCategoryByName(string name);

		void InsertCategory(string name);
		void InsertCategory(CategoryModel category);

		void UpdateCategory(CategoryModel category);
		void UpdateCategory(int id, string value);

		void DeleteCategory(CategoryModel model);
		void DeleteCategory(int categoryId);
		void DeleteCategory(string name);
	}
}
