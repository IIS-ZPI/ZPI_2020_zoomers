using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using zpi_aspnet_test.Enumerators;

namespace zpi_aspnet_test.Models
{
    public class ProductModel
    {
        public ProductCategoryEnum Category { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public double PurchasePrice { get; set; }
        public double PreferredPrice { get; set; }
        public double FinalPrice { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("------------------------------------\n");
            builder.Append($"Category: {Category}, ID: {Id}, Name: {Name}, Purchase price: {PurchasePrice}," +
                           $"Preferred price: {PreferredPrice}, Final price: {FinalPrice}\n");
            builder.Append("------------------------------------\n");

            return builder.ToString();
        }
    }
}
