using System.Collections.Generic;
using System.Linq;
using NHamcrest.Core;

namespace zpi_aspnet_test.Tests.Matchers
{
	public class CollectionMatcher<T> : Matcher<ICollection<T>>
	{
		private readonly ICollection<T> _collection;

		public CollectionMatcher(ICollection<T> collection)
		{
			_collection = collection;
		}

		public override bool Matches(ICollection<T> collection)
		{
			return collection.Count == _collection.Count &&
				   _collection.All(item => collection.Any(i => i.Equals(item)));
		}
	}
}