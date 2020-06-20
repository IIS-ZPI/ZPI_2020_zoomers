using System.Collections.Generic;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	public class StandardTaxDatabaseAccessor : ITaxDatabaseAccess
	{
		public void SetProvider(IDatabaseContextProvider provider)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<TaxModel> GetTaxes()
		{
			throw new System.NotImplementedException();
		}

		public TaxModel GetTaxById(int id)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<TaxModel> GetTaxesByCategory(CategoryModel category)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<TaxModel> GetTaxesByCategory(int categoryId)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<TaxModel> GetTaxesByCategory(string categoryName)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<TaxModel> GetTaxesByState(string stateName)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<TaxModel> GetTaxesByState(int stateId)
		{
			throw new System.NotImplementedException();
		}

		public ICollection<TaxModel> GetTaxesByState(StateOfAmericaModel state)
		{
			throw new System.NotImplementedException();
		}

		public int InsertTax(TaxModel tax)
		{
			throw new System.NotImplementedException();
		}

		public int InsertTax(int stateId, int categoryId, double taxRate, double minValue, double maxValue)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateTax(TaxModel tax)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateTax(int taxId, int stateId, int categoryId, double taxRate, double minValue, double maxValue)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteTax(TaxModel taxToDelete)
		{
			throw new System.NotImplementedException();
		}
	}
}