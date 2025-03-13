﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderProcessor.OptionsEnums;

namespace OrderProcessor.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public string ProductName { get; set; }
        public ClientType ClientType { get; set; }
        public string ShippingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreationTime { get; set; }
    }
}