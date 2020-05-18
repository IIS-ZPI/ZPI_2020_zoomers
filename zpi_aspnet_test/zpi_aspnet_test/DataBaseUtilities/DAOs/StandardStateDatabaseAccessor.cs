using System.Collections.Generic;
using System.Linq;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	//TODO surround all DB access operations into DB transactions

	public class StandardStateDatabaseAccessor : IStateDatabaseAccess
	{
		private IDatabaseContextProvider _provider;

		public StandardStateDatabaseAccessor()
		{
			_provider = DatabaseContextProvider.Instance;
		}

		public ICollection<StateOfAmericaModel> GetStates()
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var states = _provider.DatabaseContext.Query<StateOfAmericaModel>("SELECT * FROM States").ToList();
			return states;
		}

		public StateOfAmericaModel GetStateById(int id)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			return db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", id);
		}

		public StateOfAmericaModel GetStateByName(string name)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			return db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", name);
		}

		public int InsertState(StateOfAmericaModel state)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", state.Name) != null) throw new ItemAlreadyExistsException();
			db.Insert(state);
			return state.Id;
		}

		public int InsertState(string name, double? baseSalesTax = null, double? groceries = null, double? preparedFood = null,
			double? prescriptionDrug = null, double? nonPrescriptionDrug = null, double? clothing = null,
			double? intangibles = null)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", name) != null) throw new ItemAlreadyExistsException();

			var state = new StateOfAmericaModel
			{
				BaseSalesTax = baseSalesTax ?? 0,
				Groceries = groceries ?? 0,
				PreparedFood = preparedFood ?? 0,
				PrescriptionDrug = prescriptionDrug ?? 0,
				NonPrescriptionDrug = nonPrescriptionDrug ?? 0,
				Clothing = clothing ?? 0,
				Intangibles = intangibles ?? 0,
				Name = name
			};

			db.Insert(state);
			return state.Id;
		}

		public void UpdateState(StateOfAmericaModel state)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", state.Id) == null) throw new ItemNotFoundException();
			db.Update(state);
		}

		public void UpdateState(int stateId, string name = "", double? baseSalesTax = null, double? groceries = null,
			double? preparedFood = null, double? prescriptionDrug = null, double? nonPrescriptionDrug = null,
			double? clothing = null, double? intangibles = null)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if(!(db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", stateId) is StateOfAmericaModel model)) throw new ItemNotFoundException();
			model.Name = name;
			model.Groceries = groceries ?? 0;
			model.BaseSalesTax = baseSalesTax ?? 0;
			model.NonPrescriptionDrug = nonPrescriptionDrug ?? 0;
			model.PreparedFood = preparedFood ?? 0;
			model.PrescriptionDrug = prescriptionDrug ?? 0;
			model.Clothing = clothing ?? 0;
			model.Intangibles = intangibles ?? 0;
			db.Update(model);
		}

		public void DeleteState(StateOfAmericaModel state)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if ((db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", state.Id) == null)) throw new ItemNotFoundException();
			db.Delete(state);
		}

		public void DeleteState(int id)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (!(db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", id) is StateOfAmericaModel model)) throw new ItemNotFoundException();
			db.Delete(model);
		}

		public void DeleteState(string name)
		{
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			if (!(db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", name) is StateOfAmericaModel model)) throw new ItemNotFoundException();
			db.Delete(model);
		}

		public void SetProvider(IDatabaseContextProvider provider)
		{
			_provider = provider ?? throw new InvalidDatabaseOperationException("Database provider is null");
		}
	}
}