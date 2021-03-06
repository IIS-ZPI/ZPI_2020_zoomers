﻿using System.Collections.Generic;
using System.Linq;
using zpi_aspnet_test.DataBaseUtilities.Exceptions;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities.DAOs
{
	public class StandardTaxDatabaseAccessor : ITaxDatabaseAccess
	{
		private IDatabaseContextProvider _provider;

		public StandardTaxDatabaseAccessor()
		{
			_provider = DatabaseContextProvider.Instance ??
						throw new InvalidDatabaseOperationException("Database provider is null");
		}

		public void SetProvider(IDatabaseContextProvider provider)
		{
			_provider = provider;
		}

		public ICollection<TaxModel> GetTaxes()
		{
			_provider.ConnectToDb();
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			ICollection<TaxModel> taxes;

			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				taxes = db.Query<TaxModel>("SELECT * FROM TAXES").ToList();
				transaction.Complete();
			}

			_provider.DisconnectFromDb();
			return taxes;
		}

		public TaxModel GetTaxById(int id)
		{
			_provider.ConnectToDb();
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			TaxModel tax;

			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				tax = db.FirstOrDefault<TaxModel>("WHERE Id = @0", id);
				transaction.Complete();
			}

			_provider.DisconnectFromDb();
			return tax;
		}

		public ICollection<TaxModel> GetTaxesByCategory(CategoryModel category) =>
			category.Id > 0 ? GetTaxesByCategory(category.Id) : GetTaxesByCategory(category.Name);

		public ICollection<TaxModel> GetTaxesByCategory(int categoryId) =>
			GetTaxes().Where(tax => tax.CategoryId == categoryId).ToList();

		public ICollection<TaxModel> GetTaxesByCategory(string categoryName)
		{
			_provider.ConnectToDb();
			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			ICollection<TaxModel> taxes;

			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				taxes = db.Query<TaxModel>(
					"SELECT * FROM Taxes t WHERE t.CategoryId = (SELECT id FROM Categories WHERE Name LIKE @0)",
					categoryName).ToList();
				transaction.Complete();
			}

			_provider.DisconnectFromDb();
			return taxes;
		}

		public ICollection<TaxModel> GetTaxesByState(int stateId) =>
			GetTaxes().Where(tax => tax.StateId == stateId).ToList();

		public ICollection<TaxModel> GetTaxesByState(StateOfAmericaModel state) => GetTaxesByCategory(state.Id);

		public int InsertTax(TaxModel tax)
		{
			_provider.DisconnectFromDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<TaxModel>(
					"WHERE TaxRate = @0 AND MinValue = @1 AND MaxValue = @2 AND CategoryId = @3 AND StateId = @4",
					tax.TaxRate, tax.MinValue, tax.MaxValue, tax.CategoryId, tax.StateId) != null)
					throw new ItemAlreadyExistsException();
				db.Insert(tax);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();

			return tax.Id;
		}

		public int InsertTax(int stateId, int categoryId, double taxRate, double minValue, double maxValue)
		{
			var model = new TaxModel
			{
				CategoryId = categoryId,
				StateId = stateId,
				MinValue = minValue,
				MaxValue = maxValue,
				TaxRate = taxRate
			};

			return InsertTax(model);
		}

		public void UpdateTax(TaxModel tax)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<TaxModel>(
					"WHERE TaxRate = @0 AND MinValue = @1 AND MaxValue = @2 AND CategoryId = @3 AND StateId = @4",
					tax.TaxRate, tax.MinValue, tax.MaxValue, tax.CategoryId, tax.StateId) == null)
					throw new ItemNotFoundException();
				db.Update(tax);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}

		public void UpdateTax(int taxId, int stateId, int categoryId, double taxRate, double minValue, double maxValue)
		{
			var model = new TaxModel
			{
				Id = taxId,
				CategoryId = categoryId,
				StateId = stateId,
				MinValue = minValue,
				MaxValue = maxValue,
				TaxRate = taxRate
			};

			UpdateTax(model);
		}

		public void DeleteTax(TaxModel taxToDelete)
		{
			_provider.ConnectToDb();

			if (!_provider.Connected) throw new AccessToNotConnectedDatabaseException();
			var db = _provider.DatabaseContext;

			using (var transaction = db.GetTransaction())
			{
				if (db.FirstOrDefault<TaxModel>(
					"WHERE TaxRate = @0 AND MinValue = @1 AND MaxValue = @2 AND CategoryId = @3 AND StateId = @4",
					taxToDelete.TaxRate, taxToDelete.MinValue, taxToDelete.MaxValue, taxToDelete.CategoryId,
					taxToDelete.StateId) == null)
					throw new ItemNotFoundException();

				db.Delete(taxToDelete);

				transaction.Complete();
			}

			_provider.DisconnectFromDb();
		}
	}
}