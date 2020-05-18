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
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public ProductModel GetProductByName(string name)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void InsertProduct(ProductModel product)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void InsertProduct(string name, CategoryModel category, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void InsertProduct(string name, string categoryName, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void UpdateProduct(ProductModel product)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void UpdateProduct(int productId, CategoryModel category = null, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void UpdateProduct(int productId, string categoryName = "", double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		}

		public void DeleteProduct(ProductModel product)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
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