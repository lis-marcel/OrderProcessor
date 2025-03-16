using OrderProcessor.OrderOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.DTO
{
    class CustomerData
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
