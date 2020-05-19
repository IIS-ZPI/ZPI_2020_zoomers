using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Web.Mvc;
using PetaPoco;

namespace zpi_aspnet_test.Models
{
	[TableName("Products"), PrimaryKey("Id")]
	public class ProductModel
	{
		  [ResultColumn]
		  public CategoryModel Category { get; set; }

		  [PetaPoco.Column]
		  public int Id { get; set; }

		  [PetaPoco.Column("Category_id")]
		  public int CategoryId { get; set; }

		  [PetaPoco.Column]
		  public string Name { get; set; }

		  [PetaPoco.Column]
		  public double PurchasePrice { get; set; }

		  [PetaPoco.Column]
		  public double PreferredPrice { get; set; }

		  [PetaPoco.Column]
		  public double FinalPrice { get; set; }

		  [PetaPoco.Ignore]
		  public SelectList ProductList { get; set; }

		  public override string ToString()
		  {
				var builder = new StringBuilder();
				builder.Append("------------------------------------\n");
				builder.Append($"Category: {Category.Name}, ID: {Id}, Name: {Name}, Purchase price: {PurchasePrice}," +
									$"Preferred price: {PreferredPrice}, Final price: {FinalPrice}\n");
				builder.Append("------------------------------------\n");

				return builder.ToString();
		  }
	}
}
