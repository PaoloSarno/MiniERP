using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public enum OrderStatus
    {
        Draft = 0,
        Confirmed = 1,
        Paid = 2,
        ShippingRequested = 3,
        Shipped = 4,
        CancellationRequested = 5,
        Cancelled = 6
    }
}

