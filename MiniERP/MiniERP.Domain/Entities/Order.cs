using MiniERP.Domain.Entities;
using MiniERP.Domain.Enums;
using MiniERP.Domain.Exceptions;

namespace MiniErp.Domain.Entities
{
    public class Order
    {
        // Identification
        public Guid OrderId { get; private set; }
        public Guid ClientId { get; private set; }

        // Status
        public OrderStatus Status { get; private set; }

        // Snapshot pricing totals
        public decimal TotalExclVAT { get; private set; }
        public decimal TotalInclVAT { get; private set; }

        // Encapsulated list of OrderItems
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;

        // Constructor
        public Order(Guid clientId)
        {
            OrderId = Guid.NewGuid();
            ClientId = clientId;
            Status = OrderStatus.Draft;
        }

        // Business methods

        // Ajouter un produit à la commande
        public void AddItem(Product product, int quantity)
        {
            if (Status != OrderStatus.Draft)
                throw new DomainException("Order is not editable");

            if (quantity <= 0)
                throw new DomainException("Quantity must be positive");

            // Fusion des lignes si même produit + même prix + même TVA
            var existingItem = _items.FirstOrDefault(i =>
                i.ProductId == product.ProductId &&
                i.UnitPriceExclVAT == product.UnitPriceExclVAT &&
                i.VATRate == product.VATRate);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var item = new OrderItem(
                    product.ProductId,
                    quantity,
                    product.UnitPriceExclVAT,
                    product.VATRate
                );
                _items.Add(item);
            }

            RecalculateTotals();
        }

        // Supprimer un item
        public void RemoveItem(Guid productId)
        {
            if (Status != OrderStatus.Draft)
                throw new DomainException("Order is not editable");

            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
                RecalculateTotals();
            }
        }

        // Confirmer la commande
        public void Confirm()
        {
            if (!_items.Any())
                throw new DomainException("Order must contain at least one item");

            Status = OrderStatus.Confirmed;
        }

        // Calculer les totaux
        private void RecalculateTotals()
        {
            TotalExclVAT = _items.Sum(i => i.LineTotalExclVAT);
            TotalInclVAT = _items.Sum(i => i.LineTotalInclVAT);
        }
    }

}