using System;
using NHamcrest;
using NHamcrest.Core;

namespace zpi_aspnet_test.Tests
{
	public class DoesNotThrowMatcher<T> : DiagnosingMatcher<Action> where T : Exception
	{
		private Func<Exception, bool> _predicate = e => !(e is T exception);


		protected override bool Matches(Action action, IDescription mismatchDescription)
		{
			try
			{
				action();
				return true;
			}
			catch (T ex)
			{
				var obj = ex;
				mismatchDescription.AppendText("the exception should not be thrown in expected type").AppendNewLine().AppendValue(obj);
				return false;
			}
			catch (Exception ex)
			{
				if (_predicate(ex))
					return true;
				mismatchDescription.AppendText("an exception of type {0} was thrown, but did not match a predicate", ex.GetType() as object).AppendNewLine().AppendValue(ex);
			}
			return false;
		}

		public DoesNotThrowMatcher<T> With(Func<Exception, bool> predicate)
		{
			_predicate = predicate;
			return this;
		}

	}
}
