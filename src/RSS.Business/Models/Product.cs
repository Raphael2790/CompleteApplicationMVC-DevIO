using System;

namespace RSS.Business.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool Active { get; set; }
        public Guid SupplierId { get; set; }

        /* EF Relations */
        public Supplier Supplier { get; set; }
    }
}
