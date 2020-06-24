using System;

namespace zpi_aspnet_test.Tests.Matchers
{
	public static class DoesNotThrow
	{
		public static DoesNotThrowMatcher<T> An<T>() where T : Exception
		{
			return new DoesNotThrowMatcher<T>();
		}
	}
}
