using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessor.OrderOptions;

namespace OrderProcessor.BO
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ClientType ClientType { get; set; }
    }
}
