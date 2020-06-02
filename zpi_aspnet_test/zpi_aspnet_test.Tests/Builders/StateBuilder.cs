using System.Runtime.InteropServices;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Tests.Builders
{
	public class StateBuilder : IBuilder<StateOfAmericaModel>
	{
		private double _groceries;
		private double _clothing;
		private string _name;
		private double _intangibles;
		private double _baseSalesTax;
		private double _nonPrescriptionDrugs;
		private double _prescriptionDrugs;
		private double _preparedFood;
		private int _id;

		public StateOfAmericaModel Build() => new StateOfAmericaModel
		{
			Clothing = _clothing,
			Name = _name,
			Groceries = _groceries,
			Intangibles = _intangibles,
			BaseSalesTax = _baseSalesTax,
			NonPrescriptionDrug = _nonPrescriptionDrugs,
			PreparedFood = _preparedFood,
			PrescriptionDrug = _prescriptionDrugs,
			Id = _id
		};

		public static StateBuilder State() => new StateBuilder();

		public StateBuilder WithGroceriesTaxRate(double tax)
		{
			_groceries = tax;
			return this;
		}

		public StateBuilder WithClothingTaxRate(double tax)
		{
			_clothing = tax;
			return this;
		}

		public StateBuilder OfName(string name)
		{
			_name = name;
			return this;
		}

		public StateBuilder WithIntangiblesTaxRate(double tax)
		{
			_intangibles = tax;
			return this;
		}

		public StateBuilder WithBaseSalesTaxRate(double tax)
		{
			_baseSalesTax = tax;
			return this;
		}

		public StateBuilder WithNonPrescriptionDrugTaxRate(double tax)
		{
			_nonPrescriptionDrugs = tax;
			return this;
		}

		public StateBuilder WithPrescriptionDrugTaxRate(double tax)
		{
			_prescriptionDrugs = tax;
			return this;
		}

		public StateBuilder WithPreparedFoodTaxRate(double tax)
		{
			_preparedFood = tax;
			return this;
		}

		public StateBuilder OfId(int id)
		{
			_id = id;
			return this;
		}

	}
}