using System.Collections.Generic;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.Interfaces
{
	public interface ITaxDatabaseAccess : IDatabaseAccessor
	{
		ICollection<TaxModel> GetTaxes();

		TaxModel GetTaxById(int id);
		ICollection<TaxModel> GetTaxesByCategory(CategoryModel category);
		ICollection<TaxModel> GetTaxesByCategory(int categoryId);
		ICollection<TaxModel> GetTaxesByCategory(string categoryName);
		ICollection<TaxModel> GetTaxesByState(string stateName);
		ICollection<TaxModel> GetTaxesByState(int stateId);
		ICollection<TaxModel> GetTaxesByState(StateOfAmericaModel state);

		int InsertTax(TaxModel tax);
		int InsertTax(int stateId, int categoryId, double taxRate, double minValue, double maxValue);

		void UpdateTax(TaxModel tax);
		void UpdateTax(int taxId, int stateId, int categoryId, double taxRate, double minValue, double maxValue);

		void DeleteTax(TaxModel taxToDelete);
	}
}