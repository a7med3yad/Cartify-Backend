using Cartify.Application.Contracts.OrderDtos;
using Cartify.Application.Services.Interfaces;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;

namespace Cartify.Application.Services.Implementation
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TblOrder> ProcessCheckoutAsync(CheckoutDto checkoutData)
        {
            // 1. إنشاء Order جديد
            var order = new TblOrder
            {
                OrderId = GenerateOrderId(),
                CustomerId = checkoutData.UserId,
                OrderDate = DateTime.Now,
                OrderStatuesId = 1, // Pending
                PaymentTypeId = GetPaymentTypeId(checkoutData.PaymentInfo.PaymentMethod),
                ShipmentMethodId = 1, // Default shipment method
                TotalPrice = checkoutData.CartItems.Sum(item => item.Price * item.Quantity),
                Tax = CalculateTax(checkoutData.CartItems.Sum(item => item.Price * item.Quantity)),
                GrantTotal = CalculateGrandTotal(checkoutData.CartItems.Sum(item => item.Price * item.Quantity)),
                CreatedDate = DateTime.Now
            };

            // 2. إضافة OrderDetails
            foreach (var item in checkoutData.CartItems)
            {
                var orderDetail = new TblOrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    CreatedDate = DateTime.Now
                };
                order.TblOrderDetails.Add(orderDetail);

                // 3. تحديث كمية المنتج في المخزون
                await _unitOfWork.CheckoutRepository.UpdateProductQuantityAsync(item.ProductId, -item.Quantity);
            }

            // 4. حفظ الطلب في الداتابيز
            var createdOrder = await _unitOfWork.CheckoutRepository.CreateOrderAsync(order);
            await _unitOfWork.SaveChanges();

            return createdOrder;
        }

        private string GenerateOrderId()
        {
            return "ORD-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        private int GetPaymentTypeId(string paymentMethod)
        {
            return paymentMethod.ToLower() switch
            {
                "card" => 1,
                "paypal" => 2,
                "cod" => 3,
                _ => 3 // Default to COD
            };
        }

        private decimal CalculateTax(decimal subtotal)
        {
            return subtotal * 0.14m; // 14% tax
        }

        private decimal CalculateGrandTotal(decimal subtotal)
        {
            return subtotal + CalculateTax(subtotal);
        }
    }
}