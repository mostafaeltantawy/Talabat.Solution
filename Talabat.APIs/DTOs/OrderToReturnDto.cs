using Talabat.Core.Enitities.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDto
    {
        public string Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }

        // public int DeliveryMethodId { get; set; } // FK
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>(); // Navigational Property [Many]

        public decimal SubTotal { get; set; }
        // [NotMapped]
        // public decimal Total { get => SubTotal + DeliveryMethod.Cost; }

        public decimal Total { get; set; }

        public string PaymentIntentId { get; set; } 
    }
}
