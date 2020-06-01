using System;

namespace zpi_aspnet_test.DataBaseUtilities.Exceptions
{
	[Serializable]
	public class ItemAlreadyExistsException : Exception
	{
		protected ItemAlreadyExistsException(System.Runtime.Serialization.SerializationInfo serializationInfo,
			System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		public ItemAlreadyExistsException()
		{
		}

		public ItemAlreadyExistsException(string message) : base(message)
		{
		}

		public ItemAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}