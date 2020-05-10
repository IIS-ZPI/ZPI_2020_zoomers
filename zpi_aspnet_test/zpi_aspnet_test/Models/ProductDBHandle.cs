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

        SqlConnection connection = null;

        public ProductDBHandle()
        {
            this.connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Connector"].ToString());
        }

        public IEnumerable<ProductModel> GetProducts()
        {
            string query =
                "SELECT [Categories].[Name], [Products].[Id], [Products].[Name], [PurchasePrice] FROM [dbo].[Products], [dbo].[Categories]" +
                "WHERE [Category_id] = [Categories].[Id]";

            var result = connection.Query<ProductModel>(query);

            return result;
        }
    }
}