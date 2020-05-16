using System;
using PetaPoco;

namespace zpi_aspnet_test.DataBaseUtilities
{
   public class DatabaseContextProvider
   {
      public IDatabase DatabaseContext { get; private set; }

      public bool Connected { get; private set; }

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