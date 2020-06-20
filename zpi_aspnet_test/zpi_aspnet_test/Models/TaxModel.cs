using System;
using PetaPoco;

namespace zpi_aspnet_test.Models
{
	[TableName("Taxes"), PrimaryKey("Id")]
	public class TaxModel
	{
		private const double Tolerance = double.Epsilon;

		[Column] 
		public int Id { get; set; }

		[Column] 
		public int CategoryId { get; set; }

		[Column] 
		public int StateId { get; set; }

		[Column] 
		public double MinValue { get; set; }

		[Column] 
		public double MaxValue { get; set; }

		[Column] 
		public double TaxRate { get; set; }

		public bool IsMoneyInRange(double money) =>
			Math.Abs(MinValue - MaxValue) < Tolerance && Math.Abs(MinValue) < Tolerance ||
			(Math.Abs(MaxValue) > Tolerance && money >= MinValue && money <= MaxValue);
	}
}