using System.Collections.Generic;
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

		[ResultColumn] 
		public ICollection<TaxModel> TaxRates { get; set; }

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append("------------------------------------\n");
			builder.Append($"State: {Name}, Id: {Id}, Base sales tax: {BaseSalesTax}\n");
			builder.Append("------------------------------------\n");
			builder.Append("|    TaxId    |  CategoryID |   MinValue  |   MaxValue  |   TaxRate   |");
			builder.Append("-----------------------------------------------------------------------");
			foreach (var tax in TaxRates)
			{
				builder.Append(
					$"|      {tax.Id,5}  |      {tax.CategoryId,5}  |   {tax.MinValue,10:,##}|   {tax.MaxValue,10:,##}|   {tax.TaxRate,10:P}|");
				builder.Append("-----------------------------------------------------------------------");
			}

			return builder.ToString();
		}
	}
}