using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CQRSCartAPI.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        [Column("Product_Id")]
        public long ProductId { get; set; }
        [Column("Product_Name")]
        public string? ProductName { get; set; }
        [Column("Cost")]
        public long Cost { get; set; }

        [ForeignKey("Cart")]
        public long CartId { set; get; }
        public Cart? Cart { set; get; }
    }
}
