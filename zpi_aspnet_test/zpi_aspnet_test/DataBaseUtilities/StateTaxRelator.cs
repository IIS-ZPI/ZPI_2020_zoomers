using System.Collections.Generic;
using PetaPoco;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.DataBaseUtilities
{
	public class StateTaxRelator
	{
		private StateOfAmericaModel _currentState;

		public StateOfAmericaModel MapStateAndTax(StateOfAmericaModel nextState, TaxModel tax)
		{
			if (nextState == null) return _currentState;

			if (_currentState != null && _currentState.Id == nextState.Id)
			{
				_currentState.TaxRates.Add(tax);
				return null;
			}

			var previousState = _currentState;
			_currentState = nextState;
			_currentState.TaxRates = new List<TaxModel> {tax};
			return previousState;
		}
	}
}