using NHamcrest;
using NHamcrest.Core;

namespace zpi_aspnet_test.Tests.Matchers
{
	public class Has<T> : Matcher<T>
	{
		private readonly IMatcher<T> _toDecorate;

		public Has(IMatcher<T> toDecorate)
		{
			_toDecorate = toDecorate;
		}

		public override bool Matches(T item)
		{
			return _toDecorate.Matches(item);
		}

		public override void DescribeTo(IDescription description)
		{
			_toDecorate.DescribeTo(description);
		}

		public override void DescribeMismatch(T item, IDescription mismatchDescription)
		{
			_toDecorate.DescribeMismatch(item, mismatchDescription);
		}
	}
}