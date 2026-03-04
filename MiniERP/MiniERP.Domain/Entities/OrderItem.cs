    using System;
namespace MiniERP.Domain.Entities
{  
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPriceExclVAT { get; private set; }
        public decimal VATRate { get; private set; }
        public int Quantity { get; private set; }

        public OrderItem(Guid productId, string productName, decimal unitPriceExclVAT, decimal vatRate, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPriceExclVAT = unitPriceExclVAT;
            VATRate = vatRate;
            Quantity = quantity;
        }

        public decimal TotalExclVAT => UnitPriceExclVAT * Quantity;
        public decimal TotalVAT => TotalExclVAT * VATRate;
        public decimal TotalInclVAT => TotalExclVAT + TotalVAT;
    }
}
