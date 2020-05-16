using System;

namespace zpi_aspnet_test.DataBaseUtilities.Exceptions
{
	[Serializable]
	public class ItemNotFoundException : Exception
	{
		protected ItemNotFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo,
			System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{

		}

		public ItemNotFoundException()
		{
		}
	}
}