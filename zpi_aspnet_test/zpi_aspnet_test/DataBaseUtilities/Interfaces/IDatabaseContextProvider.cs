using PetaPoco;

namespace zpi_aspnet_test.DataBaseUtilities.Interfaces
{
	public interface IDatabaseContextProvider
	{
		IDatabase DatabaseContext { get; }
		bool Connected { get; }
		bool ConnectToDb();
		void DisconnectFromDb();
		string DefaultConnectionStringId { get; }
		string CurrentConnectionStringId { get; set; }
	}
}
