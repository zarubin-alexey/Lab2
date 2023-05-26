using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.DAL
{
    [Table("products")]
    public class TbProduct
    {
        [Key, Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public int count { get; set; }

        public string brand { get; set; }

        public string size { get; set; }

        public decimal price { get; set; }
    }
}
