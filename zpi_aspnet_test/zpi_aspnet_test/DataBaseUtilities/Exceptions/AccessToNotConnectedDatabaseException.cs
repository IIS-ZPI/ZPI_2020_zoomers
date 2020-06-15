using System;

namespace zpi_aspnet_test.DataBaseUtilities.Exceptions
{
	[Serializable]
	public class AccessToNotConnectedDatabaseException : Exception
	{
		protected AccessToNotConnectedDatabaseException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		public AccessToNotConnectedDatabaseException()
		{
		}

		public AccessToNotConnectedDatabaseException(string msg) : base(msg)
		{
		}

		public AccessToNotConnectedDatabaseException(string msg, Exception inner) : base(msg, inner)
		{
		}
	}
}