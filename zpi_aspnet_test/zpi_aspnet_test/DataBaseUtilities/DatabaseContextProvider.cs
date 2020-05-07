using System;
using PetaPoco;

namespace zpi_aspnet_test.DataBaseUtilities
{
   public class DatabaseContextProvider
   {
      public Database DatabaseContext { get; private set; }

      private static readonly Lazy<DatabaseContextProvider> LazyInitializer =
         new Lazy<DatabaseContextProvider>(() => new DatabaseContextProvider());

      private DatabaseContextProvider()
      {

      }

      public static DatabaseContextProvider Instance => LazyInitializer.Value;

      public bool ConnectToDb(string connectionStringId)
      {
         var rV = true;

         try
         {
            DatabaseContext = new Database(connectionStringId);
         }
         catch (Exception exception)
         {
            Console.WriteLine(exception.Source);
            Console.WriteLine(exception.StackTrace);
            rV = false;
         }

         return rV;
      }

      public void DisconnectFromDb()
      {
         DatabaseContext?.Dispose();
      }

   }
}