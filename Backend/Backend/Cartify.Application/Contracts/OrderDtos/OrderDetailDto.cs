namespace Cartify.Application.Contracts.OrderDtos
{
    public class OrderDetailDto
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? PaymentType { get; set; }
        public int? CustomerId { get; set; }
        public string? StoreName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }

}
