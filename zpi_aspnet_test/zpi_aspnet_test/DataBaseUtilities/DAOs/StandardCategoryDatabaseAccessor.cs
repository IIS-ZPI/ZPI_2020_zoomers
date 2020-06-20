using System.Collections.Generic;
using System.Linq;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	public class StandardCategoryDatabaseAccessor : ICategoryDatabaseAccess
	{
		public StandardCategoryDatabaseAccessor()
		{
			_provider = DatabaseContextProvider.Instance ??
						throw new InvalidDatabaseOperationException("Database provider is null");
		}

		private IDatabaseContextProvider _provider;

		public ICollection<CategoryModel> GetCategories()
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			ICollection<CategoryModel> categories;
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				categories = _provider.DatabaseContext.Query<CategoryModel>("SELECT * FROM Categories").ToList();
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return categories;
		}

		public CategoryModel GetCategoryById(int id)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			CategoryModel rV;

			using (var transaction = db.GetTransaction())
			{
				rV = db.FirstOrDefault<CategoryModel>("WHERE Id = @0", id);
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return rV;
		}

		public CategoryModel GetCategoryByName(string name)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			CategoryModel rV;

			using (var transaction = db.GetTransaction())
			{
				rV = db.FirstOrDefault<CategoryModel>("WHERE Name = @0", name);
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return rV;
		}

		public int InsertCategory(string name)
		{
			var category = new CategoryModel {Name = name};
			return InsertCategory(category);
		}

		public int InsertCategory(CategoryModel category)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<CategoryModel>("WHERE Name = @0", category.Name) != null)
					throw new ItemAlreadyExistsException();
				db.Insert(category);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return category.Id;
		}

		public void UpdateCategory(CategoryModel category)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<CategoryModel>("WHERE Id = @0", category.Id) == null)
					throw new ItemNotFoundException();
				db.Update(category);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void UpdateCategory(int id, string value)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<CategoryModel>("WHERE Id = @0", id) == null) throw new ItemNotFoundException();
				db.Update<CategoryModel>("SET Name = @1 WHERE Id = @0", id, value);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteCategory(CategoryModel model)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<CategoryModel>("WHERE Id = @0", model.Id) == null)
					throw new ItemNotFoundException();
				db.Delete(model);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteCategory(int categoryId)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<CategoryModel>("WHERE Id = @0", categoryId) == null)
					throw new ItemNotFoundException();
				db.Delete<CategoryModel>("WHERE Id = @0", categoryId);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteCategory(string name)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<CategoryModel>("WHERE Name = @0", name) == null)
					throw new ItemNotFoundException();
				db.Delete<CategoryModel>("WHERE Name = @0", name);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void SetProvider(IDatabaseContextProvider provider)
		{
			_provider = provider ?? throw new InvalidDatabaseOperationException("Database provider is null");
		}
	}
}