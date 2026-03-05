using MiniERP.Domain.Enums;

namespace MiniERP.Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public int StockQuantity { get; private set; }
        public ProductStatus Status { get; private set; }
        public decimal Size { get; private set; }
        public decimal UnitPriceExclVAT { get; private set; }
        public decimal VATRate { get; private set; }

        public Product(Guid id, string name, string category, decimal price, decimal vat, int stock = 0)
        {
            ProductId = id;
            Name = name;
            Category = category;
            UnitPriceExclVAT = price;
            VATRate = vat;
            StockQuantity = stock;
            Status = ProductStatus.Active;
        }

        public void Deactivate() => Status = ProductStatus.Inactive;
        public void AdjustStock(int quantity) => StockQuantity += quantity;
    }

}
