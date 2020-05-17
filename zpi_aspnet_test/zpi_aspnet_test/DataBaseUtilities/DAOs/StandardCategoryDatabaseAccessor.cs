using System.Collections.Generic;
using System.Linq;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	//TODO surround all DB access operations into DB transactions

	public class StandardCategoryDatabaseAccessor : ICategoryDatabaseAccess
	{
		public StandardCategoryDatabaseAccessor()
		{
			_provider = DatabaseContextProvider.Instance;
		}

		private IDatabaseContextProvider _provider;

		public ICollection<CategoryModel> GetCategories()
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var categories = _provider.DatabaseContext.Query<CategoryModel>("SELECT * FROM ProductCategory").ToList();
			return categories;
		}

		public CategoryModel GetCategoryById(int id)
		{
			if(!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			return db.FirstOrDefault<CategoryModel>("WHERE id = @0", id);
		}

		public CategoryModel GetCategoryByName(string name)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			return db.FirstOrDefault<CategoryModel>("WHERE name = @0", name);
		}

		public int InsertCategory(string name)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<CategoryModel>("WHERE name = @0", name) != null) throw new ItemAlreadyExistsException();
			var category = new CategoryModel(){Name = name};
			db.Insert(category);
			return category.Id;
		}

		public int InsertCategory(CategoryModel category)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if(db.FirstOrDefault<CategoryModel>("WHERE name = @0", category.Name) != null) throw new ItemAlreadyExistsException();
			db.Insert(category);
			return category.Id;
		}

		public void UpdateCategory(CategoryModel category)
		{
			if(!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if(db.FirstOrDefault<CategoryModel>("WHERE id = @0", category.Id) == null) throw new ItemNotFoundException();
			db.Update(category);
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

		public void SetProvider(IDatabaseContextProvider provider)
		{
			_provider = provider ?? throw new InvalidDatabaseOperationException("Database provider is null");
		}
	}
}