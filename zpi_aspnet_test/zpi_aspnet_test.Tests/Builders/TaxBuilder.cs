using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Tests.Builders
{
	public class TaxBuilder : IBuilder<TaxModel>
	{
		private readonly StateBuilder _parentBuilder;
		private int _id;
		private double _minValue;
		private double _maxValue;
		private double _taxRate;
		private int _stateId;
		private int _categoryId;

		public TaxBuilder(int stateId, StateBuilder stateBuilder)
		{
			_parentBuilder = stateBuilder;
			_stateId = stateId;
		}

		private TaxBuilder()
		{
		}

		public static TaxBuilder Tax() => new TaxBuilder();

		public StateBuilder ConfirmTax()
		{
			_parentBuilder.Taxes.Add(Build());
			return _parentBuilder;
		}

		public TaxModel Build()
		{
			return new TaxModel
			{
				Id = _id,
				CategoryId = _categoryId,
				MaxValue = _maxValue,
				MinValue = _minValue,
				TaxRate = _taxRate,
				StateId = _stateId
			};
		}

		public TaxBuilder OfId(int id)
		{
			_id = id;
			return this;
		}

		public TaxBuilder OfMinValue(double minValue)
		{
			_minValue = minValue;
			return this;
		}

		public TaxBuilder OfMaxValue(double maxValue)
		{
			_maxValue = maxValue;
			return this;
		}

		public TaxBuilder OfTaxRate(double taxRate)
		{
			_taxRate = taxRate;
			return this;
		}

		public TaxBuilder OfStateId(int stateId)
		{
			_stateId = stateId;
			return this;
		}

		public TaxBuilder OfCategoryId(int categoryId)
		{
			_categoryId = categoryId;
			return this;
		}
	}
}