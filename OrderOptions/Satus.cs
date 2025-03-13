using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.OptionsEnums
{
    public enum Satus
    {
        New,
        InStock,
        InShipping,
        ReturnedToSender,
        Error,
        Closed
    }
}