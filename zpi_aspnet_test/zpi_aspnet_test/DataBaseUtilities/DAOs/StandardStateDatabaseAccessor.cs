using System.Collections.Generic;
using System.Linq;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	public class StandardStateDatabaseAccessor : IStateDatabaseAccess
	{
		private IDatabaseContextProvider _provider;

		public StandardStateDatabaseAccessor()
		{
			_provider = DatabaseContextProvider.Instance ?? throw new InvalidDatabaseOperationException("Database provider is null");
		}

		public ICollection<StateOfAmericaModel> GetStates()
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			
			var db = _provider.DatabaseContext;
			List<StateOfAmericaModel> states;
			
			using (var transaction = db.GetTransaction())
			{
				states = db.Query<StateOfAmericaModel>("SELECT * FROM States").ToList();
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return states;
		}

		public StateOfAmericaModel GetStateById(int id)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();

			var db = _provider.DatabaseContext;
			StateOfAmericaModel rV;
		
			using (var transaction = db.GetTransaction())
			{
				rV = db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", id);
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return rV;
		}

		public StateOfAmericaModel GetStateByName(string name)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			StateOfAmericaModel rV;

			using (var transaction = db.GetTransaction())
			{
				rV = db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", name);
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return rV;
		}

		public int InsertState(StateOfAmericaModel state)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", state.Name) != null)
					throw new ItemAlreadyExistsException();

				db.Insert(state);
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return state.Id;
		}

		public int InsertState(string name, double? baseSalesTax = null, double? groceries = null, double? preparedFood = null,
			double? prescriptionDrug = null, double? nonPrescriptionDrug = null, double? clothing = null,
			double? intangibles = null)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			
			var db = _provider.DatabaseContext;
			StateOfAmericaModel state;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", name) != null) throw new ItemAlreadyExistsException();

				state = new StateOfAmericaModel
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

				transaction.Complete();
			}
			
			_provider.DisconnectFromDb();

			return state.Id;
		}

		public void UpdateState(StateOfAmericaModel state)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
		
			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", state.Id) == null)
					throw new ItemNotFoundException();
				db.Update(state);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void UpdateState(int stateId, string name = "", double? baseSalesTax = null, double? groceries = null,
			double? preparedFood = null, double? prescriptionDrug = null, double? nonPrescriptionDrug = null,
			double? clothing = null, double? intangibles = null)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			
			using (var transaction = db.GetTransaction())
			{
				if (!(db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", stateId) is StateOfAmericaModel model))
					throw new ItemNotFoundException();

				model.Name = name;
				model.Groceries = groceries ?? 0;
				model.BaseSalesTax = baseSalesTax ?? 0;
				model.NonPrescriptionDrug = nonPrescriptionDrug ?? 0;
				model.PreparedFood = preparedFood ?? 0;
				model.PrescriptionDrug = prescriptionDrug ?? 0;
				model.Clothing = clothing ?? 0;
				model.Intangibles = intangibles ?? 0;

				db.Update(model);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteState(StateOfAmericaModel state)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			
			using (var transaction = db.GetTransaction())
			{
				if ((db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", state.Id) == null))
					throw new ItemNotFoundException();
				db.Delete(state);
				
				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteState(int id)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			
			using (var transaction = db.GetTransaction())
			{
				if (!(db.FirstOrDefault<StateOfAmericaModel>("WHERE Id = @0", id) is StateOfAmericaModel model))
					throw new ItemNotFoundException();
				db.Delete(model);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void DeleteState(string name)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;
			
			using (var transaction = db.GetTransaction())
			{
				if (!(db.FirstOrDefault<StateOfAmericaModel>("WHERE Name = @0", name) is StateOfAmericaModel model))
					throw new ItemNotFoundException();
				db.Delete(model);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void SetProvider(IDatabaseContextProvider provider)
		{
			_provider = provider ?? throw new InvalidDatabaseOperationException("Database provider is null");
		}
	}
}