using PetaPoco;

namespace zpi_aspnet_test.Models
{
	[TableName("Taxes"), PrimaryKey("Id")]
	public class TaxModel
	{
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
	}
}