using System.Collections.Generic;
using NHamcrest;

namespace zpi_aspnet_test.Tests.Matchers
{
	public static class DecoratorMatchers
	{
		public static IMatcher<T> Is<T>(IMatcher<T> toDecorate)
		{
			return new Is<T>(toDecorate);
		}

		public static IMatcher<T> Has<T>(IMatcher<T> toDecorate)
		{
			return new Has<T>(toDecorate);
		}

		public static IMatcher<T> Is<T>(T item)
		{
			return new Is<T>(item);
		}

		public static IMatcher<ICollection<T>> CollectionEqualTo<T>(ICollection<T> items)
		{
			return new CollectionMatcher<T>(items);
		}
	}
}