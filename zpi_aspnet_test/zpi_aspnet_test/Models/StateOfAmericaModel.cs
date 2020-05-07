using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Extensions;
using PetaPoco;
namespace zpi_aspnet_test.Models
{
   [TableName("Sales"), PrimaryKey("Id")]
   public class StateOfAmericaModel
   {
      [Column]
      public int Id { get; set; }

      [Column(Name = "State")]
      public string Name { get; set; }

      [Column(Name = "Base_sales_tax")]
      public double BaseSalesTax { get; set; }

      [Ignore]
      [Obsolete]
      public double TotalTax { get; set; }

      [Column]
      public double Groceries { get; set; }

      [Column(Name = "Prepared_food")]
      public double PreparedFood { get; set; }

      [Column(Name = "Prescription drug")]
      public double PrescriptionDrug { get; set; }

      [Column(Name = "Non-prescription drug")]
      public double NonPrescriptionDrug { get; set; }

      [Column]
      public double Clothing { get; set; }

      [Column]
      public double Intangibles { get; set; }

      [Ignore]
      [Obsolete]
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