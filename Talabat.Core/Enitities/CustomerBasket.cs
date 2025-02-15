﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Enitities
{
    public class CustomerBasket
    {
        public CustomerBasket(string BasketId)
        {
            Id = BasketId;
        }
        public CustomerBasket() { }

        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
