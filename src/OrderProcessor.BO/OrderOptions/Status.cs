using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.BO.OrderOptions
{
    public enum Status
    {
        New,
        InStock,
        InShipping,
        ReturnedToCustomer,
        Error,
        Closed
    }
}