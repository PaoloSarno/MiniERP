using MiniERP.Domain.Enums;

namespace MiniErp.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public decimal TotalExclVAT => _items.Sum(i => i.LineTotalExclVAT);
        public decimal TotalInclVAT => _items.Sum(i => i.LineTotalInclVAT);

        private bool _isPaid;

        public Order(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Draft;
        }

        #region Business Methods

        public void AddItem(Guid productId, int quantity, decimal unitPriceExclVAT, decimal vatRate, ProductStatus productStatus)
        {
            if (Status != OrderStatus.Draft)
                throw new InvalidOperationException("Cannot modify a non-draft order.");

            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero.");

            if (productStatus != ProductStatus.Active)
                throw new InvalidOperationException("Cannot add inactive product to order.");

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                _items.Add(new OrderItem(productId, quantity, unitPriceExclVAT, vatRate));
            }
        }

        public void RemoveItem(Guid productId)
        {
            if (Status != OrderStatus.Draft)
                throw new InvalidOperationException("Cannot modify a non-draft order.");

            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new InvalidOperationException("Item not found in order.");

            _items.Remove(item);
        }

        public void ConfirmOrder()
        {
            if (Status != OrderStatus.Draft)
                throw new InvalidOperationException("Order already confirmed or processed.");

            if (!_items.Any())
                throw new InvalidOperationException("Order must have at least one item.");

            // Stock validation should happen in Application Layer before calling this
            Status = OrderStatus.Confirmed;
        }

        public void MarkAsPaid()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Order must be confirmed before payment.");

            if (_isPaid)
                throw new InvalidOperationException("Order is already marked as paid.");

            _isPaid = true;
        }

        public void ShipOrder()
        {
            if (!_isPaid)
                throw new InvalidOperationException("Cannot ship unpaid order.");

            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed orders can be shipped.");

            Status = OrderStatus.Shipped;
        }

        #endregion
    }

    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPriceExclVAT { get; private set; }
        public decimal VATRate { get; private set; }
        public decimal LineTotalExclVAT => Quantity * UnitPriceExclVAT;
        public decimal LineTotalInclVAT => LineTotalExclVAT * (1 + VATRate / 100);

        internal OrderItem(Guid productId, int quantity, decimal unitPriceExclVAT, decimal vatRate)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPriceExclVAT = unitPriceExclVAT;
            VATRate = vatRate;
        }

        internal void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}
