using System;

namespace Cartify.Domain.Entities
{
    public class TblCart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
