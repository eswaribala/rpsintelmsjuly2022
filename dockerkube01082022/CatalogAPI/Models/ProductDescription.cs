using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Models
{
    [Owned]
    public class ProductDescription
    {
        [Column("Summary",TypeName ="varchar(200)")]
        public String? Summary { get; set; }
        [Column("DOE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM dd yyyy}")]
        public DateTime DOE { get; set; }
        [Column("UOM")]
        public String? UOM { get; set; }

    }
}
