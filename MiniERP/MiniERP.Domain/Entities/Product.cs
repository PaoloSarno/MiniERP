using MiniERP.Domain.Enums;

namespace MiniERP.Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; private set; }

        public string Name { get; private set; }

        public decimal UnitPriceExclVAT { get; private set; }

        public decimal VATRate { get; private set; }

        public int StockQuantity { get; private set; }

        public ProductStatus Status { get; private set; }

        public Product(
            string name,
            decimal unitPriceExclVat,
            decimal vatRate,
            int stockQuantity)
        {
            ProductId = Guid.NewGuid();
            Name = name;
            UnitPriceExclVAT = unitPriceExclVat;
            VATRate = vatRate;
            StockQuantity = stockQuantity;
            Status = ProductStatus.Active;
        }

        public void DecreaseStock(int quantity)
        {
            if (StockQuantity < quantity)
                throw new InvalidOperationException("Insufficient stock");

            StockQuantity -= quantity;
        }
    }
}

