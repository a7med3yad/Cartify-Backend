using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cartify.Application.Contracts.OrderDtos
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Store ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Payment type ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Payment type ID must be greater than 0")]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "Shipment method ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Shipment method ID must be greater than 0")]
        public int ShipmentMethodId { get; set; }

        [Required(ErrorMessage = "Order items are required")]
        [MinLength(1, ErrorMessage = "At least one order item is required")]
        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();

        // Tax is optional
        public decimal? Tax { get; set; }
    }

    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "Product detail ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product detail ID must be greater than 0")]
        public int ProductDetailId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}

