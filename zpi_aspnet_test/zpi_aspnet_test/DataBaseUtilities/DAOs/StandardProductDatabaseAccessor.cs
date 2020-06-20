using System.Collections.Generic;
using System.Linq;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	//TODO surround all DB access operations into DB transactions

	public class StandardProductDatabaseAccessor : IProductDatabaseAccess
	{
		private IDatabaseContextProvider _provider;

		public StandardProductDatabaseAccessor()
		{
			_provider = DatabaseContextProvider.Instance ??
						throw new InvalidDatabaseOperationException("Database provider is null");
		}

		public ICollection<ProductModel> GetProducts()
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			ICollection<ProductModel> products;

			using (var transaction = db.GetTransaction())
			{
				products = _provider.DatabaseContext
				   .Query<ProductModel, CategoryModel>(
						"SELECT * FROM Products p LEFT JOIN CATEGORIES c ON p.Category_id = c.Id").ToList();
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return products;
		}

		public ProductModel GetProductById(int id)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			ProductModel rV;

			using (var transaction = db.GetTransaction())
			{
				rV = db.Query<ProductModel, CategoryModel>(
						"SELECT * FROM Products p LEFT JOIN CATEGORIES c ON p.Category_id = c.Id WHERE Id = @0", id)
				   .FirstOrDefault();
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return rV;
		}

		public ICollection<ProductModel> GetProductsFromCategory(CategoryModel model)
		{
			var collection = GetProducts();
			return collection.Where(productModel =>
				productModel.Category.Id == model.Id && productModel.Category.Name.Equals(model.Name)).ToList();
		}

		public ProductModel GetProductByName(string name)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			ProductModel rV;

			using (var transaction = db.GetTransaction())
			{
				rV = db.Query<ProductModel, CategoryModel>(
						"SELECT * FROM Products p LEFT JOIN CATEGORIES c ON p.Category_id = c.Id WHERE p.Name = @0",
						name)
				   .FirstOrDefault();

				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return rV;
		}

		public int InsertProduct(ProductModel product)
		{
			_provider.DisconnectFromDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<ProductModel>("WHERE Name = @0", product.Name) != null)
					throw new ItemAlreadyExistsException();
				db.Insert(product);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return product.Id;
		}

		public int InsertProduct(string name, CategoryModel category, double purchasePrice = 0,
			double preferredPrice = 0,
			double finalPrice = 0)
		{
			var product = new ProductModel
			{
				CategoryId = category.Id,
				FinalPrice = finalPrice,
				PreferredPrice = preferredPrice,
				PurchasePrice = purchasePrice,
				Name = name
			};

			return InsertProduct(product);
		}

		public int InsertProduct(string name, int categoryId, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0)
		{
			var product = new ProductModel
			{
				CategoryId = categoryId,
				FinalPrice = finalPrice,
				PreferredPrice = preferredPrice,
				PurchasePrice = purchasePrice,
				Name = name
			};

			return InsertProduct(product);
		}

		public void UpdateProduct(ProductModel product)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<ProductModel>("WHERE Name = @0", product.Name) == null)
					throw new ItemNotFoundException();
				db.Update(product);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void UpdateProduct(int productId, CategoryModel category, double purchasePrice = 0,
			double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			var product = new ProductModel
			{
				Id = productId,
				Name = productName,
				CategoryId = category.Id,
				FinalPrice = finalPrice,
				PreferredPrice = preferredPrice,
				PurchasePrice = purchasePrice
			};

			UpdateProduct(product);
		}

		public void UpdateProduct(int productId, int categoryId, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			var product = new ProductModel
			{
				Id = productId,
				Name = productName,
				CategoryId = categoryId,
				FinalPrice = finalPrice,
				PreferredPrice = preferredPrice,
				PurchasePrice = purchasePrice
			};

			UpdateProduct(product);
		}

		public void DeleteProduct(ProductModel product)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<ProductModel>("WHERE Id = @0", product.Id) == null)
					throw new ItemNotFoundException();
				db.Delete(product);
				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteProduct(int productId)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<ProductModel>("WHERE Id = @0", productId) == null)
					throw new ItemNotFoundException();
				db.Delete<ProductModel>("WHERE Id = @0", productId);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteProduct(string productName)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<ProductModel>("WHERE Name = @0", productName) == null)
					throw new ItemNotFoundException();
				db.Delete<ProductModel>("WHERE Name = @0", productName);

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