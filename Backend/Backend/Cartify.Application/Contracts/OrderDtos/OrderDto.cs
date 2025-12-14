namespace Cartify.Application.Contracts.OrderDtos
{
    public class OrderDto
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; }
    }
}
