using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CQRSCartAPI.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Cart_Id")]
        public long CartId { get; set; }
        [Column("Cart_Name")]
        public string? CartName { get; set; }
        [JsonIgnore]
        public ICollection<Product> ProductList;
    }
}
