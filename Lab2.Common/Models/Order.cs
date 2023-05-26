using System;

namespace Lab2.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime Created { get; set; }
    }
}
