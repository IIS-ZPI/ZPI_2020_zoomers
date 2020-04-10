using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Extensions;

namespace zpi_aspnet_test.Models
{
   public class StateOfAmericaModel
   {
      public string Name { get; set; }
      public double BaseSalesTax { get; set; }
      public double TotalTax { get; set; }
      public Dictionary<ProductCategoryEnum, double> Rates { get; set; }

      public override string ToString()
      {
         StringBuilder builder = new StringBuilder();
         builder.Append("------------------------------------\n");
         builder.Append($"State: {Name}, Base sales tax: {BaseSalesTax}, Total with max local surtax {TotalTax}\n");

         foreach (var (typeEnum, value) in Rates)
         {
            builder.Append($"Name of category {typeEnum}, Rate: {value}\n");
         }

         builder.Append("------------------------------------\n");

         return builder.ToString();
      }
   }
}