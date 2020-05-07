using System.Collections.Generic;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	public class StandardProductDatabaseAccessor : IProductDatabaseAccess
	{
		public ICollection<ProductModel> GetProducts()
		{
			throw new System.NotImplementedException();
		}

		public ProductModel GetProductById(int id)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<ProductModel> GetProductsFromCategory(CategoryModel model)
		{
			throw new System.NotImplementedException();
		}

		public ProductModel GetProductByName(string name)
		{
			throw new System.NotImplementedException();
		}

		public void InsertProduct(ProductModel product)
		{
			throw new System.NotImplementedException();
		}

		public void InsertProduct(string name, CategoryModel category, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0)
		{
			throw new System.NotImplementedException();
		}

		public void InsertProduct(string name, string categoryName, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateProduct(ProductModel product)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateProduct(int productId, CategoryModel category = null, double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			throw new System.NotImplementedException();
		}

		public void UpdateProduct(int productId, string categoryName = "", double purchasePrice = 0, double preferredPrice = 0,
			double finalPrice = 0, string productName = "")
		{
			throw new System.NotImplementedException();
		}

		public void DeleteProduct(ProductModel product)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteProduct(int productId)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteProduct(string productName)
		{
			throw new System.NotImplementedException();
		}
	}
}