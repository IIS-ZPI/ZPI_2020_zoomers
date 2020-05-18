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

      int InsertProduct(ProductModel product);
      int InsertProduct(string name, CategoryModel category, double purchasePrice = 0.0, double preferredPrice = 0.0, double finalPrice = 0.0);
      int InsertProduct(string name, int categoryId, double purchasePrice = 0.0, double preferredPrice = 0.0, double finalPrice = 0.0);

      void UpdateProduct(ProductModel product);
      void UpdateProduct(int productId, CategoryModel category, double purchasePrice = 0.0,
	      double preferredPrice = 0.0, double finalPrice = 0.0, string productName = "");
      void UpdateProduct(int productId, int categoryId, double purchasePrice = 0.0,
	      double preferredPrice = 0.0, double finalPrice = 0.0, string productName = "");

      void DeleteProduct(ProductModel product);
      void DeleteProduct(int productId);
      void DeleteProduct(string productName);
   }
}
