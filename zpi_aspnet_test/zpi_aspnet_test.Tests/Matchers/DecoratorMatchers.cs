using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHamcrest;
using NHamcrest.Core;

namespace zpi_aspnet_test.Tests.Matchers
{
	public static class DecoratorMatchers
	{
		public static Matcher<T> Is<T>(Matcher<T> toDecorate)
		{
			return new Is<T>(toDecorate);
		}

		public static Matcher<T> Has<T>(Matcher<T> toDecorate)
		{
			return new Has<T>(toDecorate);
		}
	}

	public class Has<T> : Matcher<T>
	{
		private readonly Matcher<T> _toDecorate;

		public Has(Matcher<T> toDecorate)
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

	public class Is<T> : Matcher<T>
	{
		private readonly Matcher<T> _decoratedMatcher;

		public Is(Matcher<T> toDecorate)
		{
			_decoratedMatcher = toDecorate;
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