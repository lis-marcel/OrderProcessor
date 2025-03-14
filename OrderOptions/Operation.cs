using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.OrderOptions
{
    public enum Operation
    {
        NewOrder,
        MoveToStock,
        MoveToShipping,
        GetAllOrders,
        Exit
    }
}