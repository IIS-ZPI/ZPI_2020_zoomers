using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Tests.Builders
{
	public class ProductBuilder : IBuilder<ProductModel>
	{
		private int _id;
		private CategoryModel _category;
		private int _categoryId;
		private double _finalPrice;
		private double _preferredPrice;
		private string _name;
		private double _purchasePrice;

		public static ProductBuilder Product()
		{
			return new ProductBuilder();
		}

		public ProductBuilder WithId(int id)
		{
			_id = id;
			return this;
		}

		public ProductBuilder OfCategory(CategoryModel category)
		{
			_category = category;
			return this;
		}

		public ProductBuilder WithCategoryId(int id)
		{
			_categoryId = id;
			return this;
		}

		public ProductBuilder WithFinalPrice(double price)
		{
			_finalPrice = price;
			return this;
		}

		public ProductBuilder WithPreferredPrice(double price)
		{
			_preferredPrice = price;
			return this;
		}

		public ProductBuilder OfName(string name)
		{
			_name = name;
			return this;
		}

		public ProductBuilder WithPurchasePrice(double price)
		{
			_purchasePrice = price;
			return this;
		}

		public ProductModel Build()
		{
			return new ProductModel
			{
				PreferredPrice = _preferredPrice,
				FinalPrice = _finalPrice,
				PurchasePrice = _purchasePrice,
				Category = _category,
				CategoryId = _categoryId,
				Name = _name,
				Id = _id
			};
		}
	}
}