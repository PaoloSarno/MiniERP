using MiniERP.Domain.Exceptions;

namespace MiniERP.Domain.Entities
{
    public class OrderItem
    {
        public Guid ProductId { get; }

        public int Quantity { get; private set; }

        public decimal UnitPriceExclVAT { get; }

        public decimal VATRate { get; }

        public decimal LineTotalExclVAT => UnitPriceExclVAT * Quantity;

        public decimal LineTotalInclVAT => LineTotalExclVAT * (1 + VATRate);

        internal OrderItem(
            Guid productId,
            int quantity,
            decimal unitPriceExclVat,
            decimal vatRate)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be positive");

            ProductId = productId;
            Quantity = quantity;
            UnitPriceExclVAT = unitPriceExclVat;
            VATRate = vatRate;
        }

        // Méthode pour augmenter la quantité lors de la fusion de lignes
        internal void IncreaseQuantity(int additionalQuantity)
        {
            if (additionalQuantity <= 0)
                throw new DomainException("Additional quantity must be positive");

            Quantity += additionalQuantity;
        }

        // Méthode pour réduire la quantité si nécessaire
        internal void DecreaseQuantity(int removeQuantity)
        {
            if (removeQuantity <= 0)
                throw new DomainException("Remove quantity must be positive");

            if (removeQuantity > Quantity)
                throw new DomainException("Cannot remove more than existing quantity");

            Quantity -= removeQuantity;
        }
    }
}
