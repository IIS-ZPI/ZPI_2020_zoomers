using System;

namespace zpi_aspnet_test.Tests
{
	public class DoesNotThrow
	{
		public static DoesNotThrowMatcher<T> An<T>() where T : Exception
		{
			return new DoesNotThrowMatcher<T>();
		}
	}
}
