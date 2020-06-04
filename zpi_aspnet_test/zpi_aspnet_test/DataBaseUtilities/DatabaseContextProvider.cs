using System;
using PetaPoco;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;

namespace zpi_aspnet_test.DataBaseUtilities
{
   public class DatabaseContextProvider : IDatabaseContextProvider
   {
      public string DefaultConnectionStringId { get; }

      public string CurrentConnectionStringId { get; set; }

      public IDatabase DatabaseContext { get; private set; }

      public bool Connected { get; private set; }

      private static readonly Lazy<DatabaseContextProvider> LazyInitializer =
         new Lazy<DatabaseContextProvider>(() => new DatabaseContextProvider());

      private DatabaseContextProvider()
      {
	      DefaultConnectionStringId = "zoomers_sql_server";
	      CurrentConnectionStringId = DefaultConnectionStringId;
      }

      public static DatabaseContextProvider Instance => LazyInitializer.Value;

      public bool ConnectToDb()
      {
         var rV = true;

         try
         {
            DatabaseContext = new Database(string.IsNullOrEmpty(CurrentConnectionStringId) ? DefaultConnectionStringId : CurrentConnectionStringId);
         }
         catch (Exception exception)
         {
            Console.WriteLine(exception.Source);
            Console.WriteLine(exception.StackTrace);
            rV = false;
         }

         Connected = rV;

         return rV;
      }

      public void DisconnectFromDb()
      {
         DatabaseContext?.Dispose();
         Connected = false;
      }

   }
}