using System.Collections.Generic;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.Interfaces
{
	public interface IStateDatabaseAccess : IDatabaseAccessor
	{
		ICollection<StateOfAmericaModel> GetStates();
		StateOfAmericaModel GetStateById(int id);
		StateOfAmericaModel GetStateByName(string name);

		int InsertState(StateOfAmericaModel state);
		int InsertState(string name, double? baseSalesTax = null, double? groceries = null, double? preparedFood = null,
			double? prescriptionDrug = null, double? nonPrescriptionDrug = null, double? clothing = null,
			double? intangibles = null);

		void UpdateState(StateOfAmericaModel state);
		void UpdateState(int stateId, string name = "", double? baseSalesTax = null, double? groceries = null,
			double? preparedFood = null,
			double? prescriptionDrug = null, double? nonPrescriptionDrug = null, double? clothing = null,
			double? intangibles = null);

		void DeleteState(StateOfAmericaModel state);
		void DeleteState(int id);
		void DeleteState(string name);
	}
}
