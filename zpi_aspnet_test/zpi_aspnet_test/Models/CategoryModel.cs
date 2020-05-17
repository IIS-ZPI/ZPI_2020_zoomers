using PetaPoco;

namespace zpi_aspnet_test.Models
{
   [TableName("Categories"), PrimaryKey("Id")]
   public class CategoryModel
   {
      public int Id { get; set; }

      public string Name { get; set; }
   }
}