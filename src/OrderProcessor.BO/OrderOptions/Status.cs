using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.BO.OrderOptions
{
    public enum Status
    {
        New = 1,
        InWarehouse = 2,
        InShipping = 3,
        ReturnedToCustomer = 4,
        Error = 5,
        Closed = 6
    }
}