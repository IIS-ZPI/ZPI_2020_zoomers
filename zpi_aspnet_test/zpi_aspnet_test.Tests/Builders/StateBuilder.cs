using System.Collections.Generic;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Tests.Builders
{
	public class StateBuilder : IBuilder<StateOfAmericaModel>
	{
		private string _name;
		private double _baseSalesTax;
		private int _id;

		internal readonly List<TaxModel> Taxes = new List<TaxModel>();

		public StateOfAmericaModel Build() => new StateOfAmericaModel
		{
			Name = _name,
			BaseSalesTax = _baseSalesTax,
			TaxRates = Taxes,
			Id = _id
		};

		public static StateBuilder State() => new StateBuilder();

		public TaxBuilder AddTax() => new TaxBuilder(_id, this);

		public StateBuilder OfBaseSalesTax(double tax)
		{
			_baseSalesTax = tax;
			return this;
		}

		public StateBuilder OfName(string name)
		{
			_name = name;
			return this;
		}

		public StateBuilder OfId(int id)
		{
			_id = id;
			return this;
		}
	}
}