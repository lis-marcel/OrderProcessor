using OrderProcessor.BO.OrderOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
