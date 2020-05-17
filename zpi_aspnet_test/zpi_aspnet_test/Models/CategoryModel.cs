using PetaPoco;

namespace zpi_aspnet_test.Models
{
   [TableName("Categories"), PrimaryKey("Id")]
   public class CategoryModel
   {
      [Column]
      public int Id { get; set; }
      [Column]
      public string Name { get; set; }
   }
}