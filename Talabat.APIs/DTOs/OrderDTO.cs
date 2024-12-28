using System.ComponentModel.DataAnnotations;
using Talabat.Core.Enitities.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketId { get; set; }
        [Required]

        public int DeliveryMethodId { get; set; }
        [Required]

        public AddressDto shipToAddress { get; set; }

    }
}
