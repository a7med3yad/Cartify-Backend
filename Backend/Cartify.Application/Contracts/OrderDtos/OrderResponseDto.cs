using System;
using System.Collections.Generic;

namespace Cartify.Application.Contracts.OrderDtos
{
    public class OrderResponseDto
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public string PaymentType { get; set; }
        public string ShipmentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrantTotal { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}

