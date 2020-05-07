using System.Collections.Generic;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	public class StandardStateDatabaseAccessor : IStateDatabaseAccess
	{
		public ICollection<StateOfAmericaModel> GetStates()
		{
			throw new System.NotImplementedException();
		}

		public StateOfAmericaModel GetStateById(int id)
		{
			throw new System.NotImplementedException();
		}

		public StateOfAmericaModel GetStateByName(string name)
		{
			throw new System.NotImplementedException();
		}

		public void InsertState(StateOfAmericaModel state)
		{
			throw new System.NotImplementedException();
		}

		public void InsertState(string name, double? baseSalesTax = null, double? groceries = null, double? preparedFood = null,
			double? prescriptionDrug = null, double? nonPrescriptionDrug = null, double? clothing = null,
			double? intangibles = null)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateState(StateOfAmericaModel state)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateState(int stateId, string name = "", double? baseSalesTax = null, double? groceries = null,
			double? preparedFood = null, double? prescriptionDrug = null, double? nonPrescriptionDrug = null,
			double? clothing = null, double? intangibles = null)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteState(StateOfAmericaModel state)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteState(int id)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteState(string name)
		{
			throw new System.NotImplementedException();
		}
	}
}