using PetaPoco;

namespace zpi_aspnet_test.Models
{
   [TableName("ProductCategory"), PrimaryKey("id")]
   public class CategoryModel
   {
      [Column(Name = "id")]
      public int Id { get; set; }

      [Column(Name = "name")]
      public string Name { get; set; }
   }
}