using MiniERP.Domain.Entities;

namespace MiniERP.Domain.Policies
{
    public interface IShipmentPolicy
    {
        /// <summary>
        /// Returns true if shipment is allowed for the given order
        /// according to company rules (stock, delay, payment, etc.)
        /// </summary>
        bool CanShip(Order order);
    }
}
