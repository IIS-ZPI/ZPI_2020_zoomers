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
			_provider = DatabaseContextProvider.Instance;
		}

		public ICollection<ProductModel> GetProducts()
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var products = _provider.DatabaseContext.Query<ProductModel, CategoryModel>("SELECT * FROM Products p LEFT JOIN CATEGORIES c ON p.Category_id = c.Id").ToList();
			return products;
		}

		public ProductModel GetProductById(int id)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			return db.Query<ProductModel, CategoryModel>("SELECT * FROM Products p LEFT JOIN CATEGORIES c ON p.Category_id = c.Id WHERE Id = @0", id).FirstOrDefault();
		}

		public ICollection<ProductModel> GetProductsFromCategory(CategoryModel model)
		{
			var collection = GetProducts();
			return collection.Where(productModel =>
				productModel.Category.Id == model.Id && productModel.Category.Name.Equals(model.Name)).ToList();
		}

		public ProductModel GetProductByName(string name)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			return db.Query<ProductModel, CategoryModel>("SELECT * FROM Products p LEFT JOIN CATEGORIES c ON p.Category_id = c.Id WHERE Name = @0", name).FirstOrDefault();
		}

		public int InsertProduct(ProductModel product)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<ProductModel>("WHERE Name = @0", product.Name) != null) throw new ItemAlreadyExistsException();
			db.Insert(product);
			return product.Id;
		}

		public int InsertProduct(string name, CategoryModel category, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<ProductModel>("WHERE Name = @0", name) != null) throw new ItemAlreadyExistsException();
			var product = new ProductModel(){CategoryId = category.Id, FinalPrice = finalPrice, PreferredPrice = preferredPrice, PurchasePrice = purchasePrice, Name = name};
			db.Insert(product);
			return product.Id;
		}

		public int InsertProduct(string name, int categoryId, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<ProductModel>("WHERE Name = @0", name) != null) throw new ItemAlreadyExistsException();
			var product = new ProductModel() { CategoryId = categoryId, FinalPrice = finalPrice, PreferredPrice = preferredPrice, PurchasePrice = purchasePrice, Name = name };
			db.Insert(product);
			return product.Id;
		}

		public void UpdateProduct(ProductModel product)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<ProductModel>("WHERE Name = @0", product.Name) == null) throw new ItemNotFoundException();
			db.Update(product);
		}

		public void UpdateProduct(int productId, CategoryModel category, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<ProductModel>("WHERE Id = @0", productId) == null) throw new ItemNotFoundException();
			var product = new ProductModel(){Id = productId, Name = productName, CategoryId = category.Id, FinalPrice = finalPrice, PreferredPrice = preferredPrice, PurchasePrice = purchasePrice};
			db.Update(product);
		}

		public void UpdateProduct(int productId, int categoryId, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<ProductModel>("WHERE Id = @0", productId) == null) throw new ItemNotFoundException();
			var product = new ProductModel() { Id = productId, Name = productName, CategoryId = categoryId, FinalPrice = finalPrice, PreferredPrice = preferredPrice, PurchasePrice = purchasePrice };
			db.Update(product);
		}

		public void DeleteProduct(ProductModel product)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if(db.FirstOrDefault<ProductModel>("WHERE Id = @0", product.Id) == null) throw new ItemNotFoundException();
			db.Delete(product);
		}

		public void DeleteProduct(int productId)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void DeleteProduct(string productName)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void SetProvider(IDatabaseContextProvider provider)
		{
			_provider = provider ?? throw new InvalidDatabaseOperationException("Database provider is null");
		}
	}
}