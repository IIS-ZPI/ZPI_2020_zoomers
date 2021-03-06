﻿using System.Text;
using PetaPoco;

namespace zpi_aspnet_test.Models
{
	[TableName("Products"), PrimaryKey("Id")]
	public class ProductModel
	{
		  [ResultColumn]
		  public CategoryModel Category { get; set; }

		  [Column]
		  public int Id { get; set; }

		  [Column("Category_id")]
		  public int CategoryId { get; set; }

		  [Column]
		  public string Name { get; set; }

		  [Column]
		  public double PurchasePrice { get; set; }

		  [Column]
		  public double PreferredPrice { get; set; }

		  [Column]
		  public double FinalPrice { get; set; }

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
