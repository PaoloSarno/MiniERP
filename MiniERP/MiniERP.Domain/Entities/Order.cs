using MiniERP.Domain.ValueObjects;
using MiniERP.Domain.Entities;
using MiniERP.Domain.Enums;
using MiniERP.Domain.Policies;

public class Order
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public Money TotalExclVAT => _items.Select(i => i.Total).Aggregate((a, b) => a + b);
    public decimal VATRate => _items.FirstOrDefault()?.UnitPrice.VATRate ?? 0;
    public Money TotalInclVAT => new Money(TotalExclVAT.AmountExclVAT, VATRate, TotalExclVAT.Currency);

    // Constructor
    public Order(Guid clientId)
    {
        Id = Guid.NewGuid();
        ClientId = clientId;
    }

    // Add item
    public void AddItem(OrderItem item)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Cannot add items once order is confirmed or beyond.");
        _items.Add(item);
    }

    // Confirm
    public void Confirm()
    {
        if (!_items.Any())
            throw new InvalidOperationException("Order must contain at least one item.");
        Status = OrderStatus.Confirmed;
        _domainEvents.Add(new OrderConfirmedEvent(Id));
    }

    // Pay
    public void Pay()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Order must be confirmed before payment.");
        Status = OrderStatus.Paid;
        _domainEvents.Add(new OrderPaidEvent(Id));
    }

    // Request shipment (policy injected)
    public void RequestShipment(IShipmentPolicy shipmentPolicy)
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Order must be paid before requesting shipment.");

        if (!shipmentPolicy.CanShip(this))
            throw new InvalidOperationException("Shipment not allowed by company policy.");

        Status = OrderStatus.ShippingRequested;
        _domainEvents.Add(new ShipmentRequestedEvent(Id));
    }

    // Confirm shipment
    public void ConfirmShipment()
    {
        if (Status != OrderStatus.ShippingRequested)
            throw new InvalidOperationException("Shipment cannot be confirmed in the current state.");

        Status = OrderStatus.Shipped;
        _domainEvents.Add(new OrderShippedEvent(Id));
    }

    // Cancel
    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new InvalidOperationException("Cannot cancel a shipped order.");

        Status = OrderStatus.Cancelled;
        // Optionnel : OrderCancelledEvent
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
