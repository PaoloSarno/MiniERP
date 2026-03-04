using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Orders
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public decimal TotalExclVAT { get; private set; }
        public decimal TotalVAT { get; private set; }
        public decimal TotalInclVAT { get; private set; }

        private readonly List<object> _domainEvents;
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        public Order(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Draft;
            _orderItems = new List<OrderItem>();
            _domainEvents = new List<object>();
        }

        #region Order Items Management

        public void AddOrderItem(OrderItem item)
        {
            if (Status != OrderStatus.Draft)
                throw new InvalidOperationException("Cannot add items to a non-draft order.");

            _orderItems.Add(item);
            RecalculateTotals();
        }

        public void RemoveOrderItem(OrderItem item)
        {
            if (Status != OrderStatus.Draft)
                throw new InvalidOperationException("Cannot remove items from a non-draft order.");

            _orderItems.Remove(item);
            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            TotalExclVAT = _orderItems.Sum(i => i.UnitPriceExclVAT * i.Quantity);
            TotalVAT = _orderItems.Sum(i => i.UnitPriceExclVAT * i.Quantity * i.VATRate);
            TotalInclVAT = TotalExclVAT + TotalVAT;
        }

        #endregion

        #region Workflow

        public void ConfirmOrder()
        {
            if (Status != OrderStatus.Draft)
                throw new InvalidOperationException("Only Draft orders can be confirmed.");
            if (!_orderItems.Any())
                throw new InvalidOperationException("Cannot confirm an empty order.");

            Status = OrderStatus.Confirmed;
        }

        public void MarkAsPaid()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Only Confirmed orders can be marked as Paid.");

            Status = OrderStatus.Paid;
            _domainEvents.Add(new OrderPaidDomainEvent(Id));
        }

        public void RequestShipping()
        {
            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException("Only Paid orders can request shipping.");

            Status = OrderStatus.ShippingRequested;
            _domainEvents.Add(new ShippingRequestedDomainEvent(Id));
        }

        public void ConfirmShipment()
        {
            if (Status == OrderStatus.Shipped)
                return; // Idempotent
            if (Status != OrderStatus.ShippingRequested)
                throw new InvalidOperationException("Cannot confirm shipment: Shipping has not been requested.");

            Status = OrderStatus.Shipped;
        }

        public void RequestCancellation()
        {
            if (Status == OrderStatus.Shipped)
                throw new InvalidOperationException("Cannot request cancellation after shipment.");
            if (Status == OrderStatus.CancellationRequested || Status == OrderStatus.Cancelled)
                return; // Idempotent

            Status = OrderStatus.CancellationRequested;
        }

        public void CancelOrder()
        {
            if (Status == OrderStatus.Shipped)
                throw new InvalidOperationException("Cannot cancel a shipped order.");

            Status = OrderStatus.Cancelled;
        }

        #endregion
    }

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
    }

    // Domain Events
    public record OrderPaidDomainEvent(Guid OrderId);
    public record ShippingRequestedDomainEvent(Guid OrderId);
}
