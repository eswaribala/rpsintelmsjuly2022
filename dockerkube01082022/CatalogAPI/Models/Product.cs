using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Models
{
    public enum ProductType { Regular, Seasonal}
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Product_Id")]
        public long ProductId { get; set; }
        [Column("Product_Type")]
        public ProductType ProductType { get; set; }
        public ProductDescription? ProductDescription { get; set; }
        [Column("Price")]
        public long Price { get; set; }

        [ForeignKey("Catalog")]
        [Column("Catalog_Id_FK")]
        public long CatalogId { get; set; }
        public Catalog? Catalog { get; set; }

    }
}
