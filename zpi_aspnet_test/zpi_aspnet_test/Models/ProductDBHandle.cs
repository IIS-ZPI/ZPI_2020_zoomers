using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace zpi_aspnet_test.Models
{
    public class ProductDBHandle
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Connector"].ToString());

        public IEnumerable<ProductModel> GetProducts()
        {
            string query =
                "SELECT [ProductCategory].[name], [ProductModel].[id], [ProductModel].[Name], [PurchasePrice] FROM [dbo].[ProductModel], [dbo].[ProductCategory]" +
                "WHERE [Category_id] = [ProductCategory].[id]";

            var result = connection.Query<ProductModel>(query);

            return result;
        }
    }
}