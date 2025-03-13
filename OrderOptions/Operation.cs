using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.OptionsEnums
{
    public enum Operation
    {
        NewOrder,
        TransferToStock,
        TransferToShipping,
        GetAllOrders,
        Exit
    }
}