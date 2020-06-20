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
			_provider = DatabaseContextProvider.Instance ??
						throw new InvalidDatabaseOperationException("Database provider is null");
		}

		public ICollection<StateOfAmericaModel> GetStates()
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();

			var db = _provider.DatabaseContext;
			List<StateOfAmericaModel> states;

			using (var transaction = db.GetTransaction())
			{
				states = db.Query<StateOfAmericaModel, TaxModel, StateOfAmericaModel>(
					new StateTaxRelator().MapStateAndTax,
					"SELECT * FROM States s LEFT JOIN Taxes t ON s.Id = t.StateId").ToList();
				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return states;
		}

		public StateOfAmericaModel GetStateById(int id) => GetStates().FirstOrDefault(state => state.Id == id);

		public StateOfAmericaModel GetStateByName(string name) =>
			GetStates().FirstOrDefault(state => state.Name.Equals(name));


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

		public int InsertState(string name, double? baseSalesTax = null)
		{
			var state = new StateOfAmericaModel
			{
				BaseSalesTax = baseSalesTax ?? 0,
				Name = name
			};

			return InsertState(state);
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

		public void UpdateState(int stateId, string name = "", double? baseSalesTax = null)
		{
			var state = new StateOfAmericaModel
			{
				BaseSalesTax = baseSalesTax ?? 0,
				Name = name
			};

			UpdateState(state);
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