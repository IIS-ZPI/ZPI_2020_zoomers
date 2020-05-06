using System;
using PetaPoco;

namespace zpi_aspnet_test.DataBaseUtilities
{
   public class DatabaseConnector
   {
      public Database ZoomersDatabase { get; private set; } = null;
      private static readonly DatabaseConnector _instance = null;


      private DatabaseConnector()
      {

      }

      private static class Initializer
      {
         static Initializer()
         {

         }

         internal static DatabaseConnector Instance => _instance ?? new DatabaseConnector();
      }

      public static DatabaseConnector Instance => Initializer.Instance;

      public bool ConnectToDb()
      {
         var rV = true;

         try
         {
            ZoomersDatabase = new Database("zoomers_sql_server");
         }
         catch (Exception exception)
         {
            Console.WriteLine(exception.Source);
            Console.WriteLine(exception.StackTrace);
            rV = false;
         }

         return rV;
      }

   }
}