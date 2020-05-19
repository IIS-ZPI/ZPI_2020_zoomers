using PetaPoco;

namespace zpi_aspnet_test.DataBaseUtilities.Interfaces
{
	public interface IDatabaseContextProvider
	{
		IDatabase DatabaseContext { get; }
		bool Connected { get; }
		bool ConnectToDb(string connectionStringId);
		void DisconnectFromDb();
	}
}
