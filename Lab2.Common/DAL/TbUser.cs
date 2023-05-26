using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.DAL
{
    [Table("users")]
    public class TbUser
    {
        [Key, Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string surname { get; set; }

        public string phone { get; set; }
    }
}
