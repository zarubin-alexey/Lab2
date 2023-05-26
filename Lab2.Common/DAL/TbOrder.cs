using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.DAL
{
    [Table("orders")]
    public class TbOrder
    {
        [Key, Required]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        public int product_id { get; set; }

        [Required]
        public int count { get; set; }

        public string address { get; set; }

        public string phone { get; set; }

        public DateTime created { get; set; }
    }
}
