namespace MiniERP.Domain.Errors
{
    public static class DomainErrorMessages
    {
        public const string InvalidQuantity = "Quantity must be greater than zero.";
        public const string ProductInactive = "Product is not active and cannot be added to order.";
        public const string InsufficientStock = "Insufficient stock for the requested product.";
        public const string OrderEmpty = "Order must contain at least one item.";
        public const string ClientInactive = "Client is not active.";
        public const string CannotConfirmShippedOrder = "Shipped orders cannot be confirmed.";
        public const string PaymentAlreadyProcessed = "Payment has already been processed.";
        public const string AddressRequired = "An address is required for this operation.";
        public const string UnauthorizedAction = "User is not authorized to perform this action.";
    }
}
