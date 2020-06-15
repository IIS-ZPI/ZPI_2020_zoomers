using System.Text;
using PetaPoco;

namespace zpi_aspnet_test.Models
{
   [TableName("States"), PrimaryKey("Id")]
   public class StateOfAmericaModel
   {
      [Column]
      public int Id { get; set; }

      [Column]
      public string Name { get; set; }

      [Column]
      public double BaseSalesTax { get; set; }
      
      [Column]
      public double Groceries { get; set; }

      [Column]
      public double PreparedFood { get; set; }

      [Column]
      public double PrescriptionDrug { get; set; }

      [Column]
      public double NonPrescriptionDrug { get; set; }

      [Column]
      public double Clothing { get; set; }

      [Column]
      public double Intangibles { get; set; }

      public override string ToString()
      {
         StringBuilder builder = new StringBuilder();
         builder.Append("------------------------------------\n");
         builder.Append($"State: {Name}, Id: {Id}, Base sales tax: {BaseSalesTax}\n");
         builder.Append($"Groceries: {Groceries}\n");
         builder.Append($"Prepared Food: {PreparedFood}\n");
         builder.Append($"Prescription Drug: {PrescriptionDrug}\n");
         builder.Append($"Non-Prescription Drug: {NonPrescriptionDrug}\n");
         builder.Append($"Clothing: {Clothing}\n");
         builder.Append($"Intangibles: {Intangibles}\n");
         builder.Append("------------------------------------\n");

         return builder.ToString();
      }
   }
}