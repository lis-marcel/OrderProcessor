using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.BO.OrderOptions
{
    public enum OrderStatus
    {
        New = 1,
        InWarehouse = 2,
        PendingToShipping = 3,
        InShipping = 4,
        ReturnedToCustomer = 5,
        Error = 6,
        Closed = 7
    }
}