using System;
using NHamcrest;
using NHamcrest.Core;

namespace zpi_aspnet_test.Tests.Matchers
{
	public class Is<T> : Matcher<T>
	{
		private readonly IMatcher<T> _decoratedMatcher;

		public Is(IMatcher<T> toDecorate)
		{
			_decoratedMatcher = toDecorate;
		}

		public static IMatcher<T> IsA(Type type)
		{
			return new Is<T>(new IsInstanceOf<T>());
		}

		public Is(T item)
		{
			_decoratedMatcher = Is.EqualTo(item);
		}

		public override bool Matches(T item)
		{
			return _decoratedMatcher.Matches(item);
		}

		public override void DescribeTo(IDescription description)
		{
			_decoratedMatcher.DescribeTo(description);
		}

		public override void DescribeMismatch(T item, IDescription mismatchDescription)
		{
			_decoratedMatcher.DescribeMismatch(item, mismatchDescription);
		}
	}
}