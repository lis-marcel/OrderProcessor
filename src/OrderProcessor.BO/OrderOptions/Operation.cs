using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.BO.OrderOptions
{
    public enum Operation
    {
        NewOrder,
        MoveToStock,
        MoveToShipping,
        ShowSpecificOrder,
        GetAllOrders,
        ChangeOrderStatus,
        Exit
    }
}