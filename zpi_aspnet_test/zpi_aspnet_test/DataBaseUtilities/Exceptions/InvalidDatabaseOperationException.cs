using System;
namespace zpi_aspnet_test.DataBaseUtilities.Exceptions
{
	[Serializable]
	public class InvalidDatabaseOperationException : Exception
	{
		protected InvalidDatabaseOperationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		public InvalidDatabaseOperationException()
		{
		}

		public InvalidDatabaseOperationException(string msg) : base(msg)
		{
		}

		public InvalidDatabaseOperationException(string msg, Exception inner) : base(msg, inner)
		{
		}
	}
}