using System.Collections.Generic;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.Interfaces
{
   public interface IProductDatabaseAccess
   {
      ICollection<ProductModel> GetProducts();
      ProductModel GetProductById(int id);
      ICollection<ProductModel> GetProductsFromCategory(CategoryModel model);
      ProductModel GetProductByName(string name);
   }
}
