using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Enitities;
using Talabat.Core.Enitities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;
using Product = Talabat.Core.Enitities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var CustomerBasket = await _basketRepository.GetBasketAsync(BasketId);
            if (CustomerBasket == null) return null;

            var ShippingPrice = 0M;
            if (CustomerBasket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod =  await _unitOfWork.Repository<DeliveryMethod>().GetById(CustomerBasket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost ; 
            }

            if (CustomerBasket.Items.Count > 0) 
            {
                foreach (var item in CustomerBasket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetById(item.Id);  
                    if(item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }
            var SubTotal = CustomerBasket.Items.Sum(item =>item.Price * item.Quantity);
            var Service = new PaymentIntentService();
            PaymentIntent PaymentIntent;
            if (string.IsNullOrEmpty(CustomerBasket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>{"card"}

                };
              PaymentIntent = await  Service.CreateAsync(Options);
                CustomerBasket.PaymentIntentId = PaymentIntent.Id;
                CustomerBasket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else 
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),

                };
                PaymentIntent =  await Service.UpdateAsync(CustomerBasket.PaymentIntentId, Options);
                CustomerBasket.PaymentIntentId = PaymentIntent.Id;
                CustomerBasket.ClientSecret = PaymentIntent.ClientSecret;
            }
           return await _basketRepository.UpdateBasketAsync(CustomerBasket);


        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool Flag)
        {
            var Spec = new OrderWithPaymnetIntentIdSpec(PaymentIntentId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(Spec);
            if (Flag) 
            {
                Order.Status = OrderStatus.PaymentRecived;
            }
            else 
            {
                Order.Status = OrderStatus.PaymentFailed;

            }
            _unitOfWork.Repository<Order>().Update(Order);
             await _unitOfWork.CompleteAsync();
            return Order;
        }

    }
}
