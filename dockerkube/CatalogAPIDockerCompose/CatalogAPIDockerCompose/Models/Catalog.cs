using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPIDockerCompose.Models
{
    [Table("Catalog")]
    public class Catalog
    {
        [Key]      
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Catalog_Id")]
        public long CatalogId { get; set; }
        [Column("Catalog_Name",TypeName ="varchar(50)")]
        public string? CatalogName { get; set; }
    }
}
