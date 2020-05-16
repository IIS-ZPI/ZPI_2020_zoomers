using System.Collections.Generic;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.Interfaces
{
   public interface IProductDatabaseAccess : IDatabaseAccessor
   {
      ICollection<ProductModel> GetProducts();
      ProductModel GetProductById(int id);
      ICollection<ProductModel> GetProductsFromCategory(CategoryModel model);
      ProductModel GetProductByName(string name);

      void InsertProduct(ProductModel product);
      void InsertProduct(string name, CategoryModel category, double purchasePrice = 0.0, double preferredPrice = 0.0, double finalPrice = 0.0);
      void InsertProduct(string name, string categoryName, double purchasePrice = 0.0, double preferredPrice = 0.0, double finalPrice = 0.0);

      void UpdateProduct(ProductModel product);
      void UpdateProduct(int productId, CategoryModel category = null, double purchasePrice = 0.0,
	      double preferredPrice = 0.0, double finalPrice = 0.0, string productName = "");
      void UpdateProduct(int productId, string categoryName = "", double purchasePrice = 0.0,
	      double preferredPrice = 0.0, double finalPrice = 0.0, string productName = "");

      void DeleteProduct(ProductModel product);
      void DeleteProduct(int productId);
      void DeleteProduct(string productName);
   }
}
