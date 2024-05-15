﻿using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class OrderResponse
    {
        public Order Order { get; set; } = null!;
        public List<OrderDetail>? OrderDetails { get; set; }    
    }
}