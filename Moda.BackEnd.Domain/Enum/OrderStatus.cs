﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Enum
{
    public enum OrderStatus
    {
        PENDING,
        PREPARING,
        TRANSFERRED_TO_SHIPPING_UNIT,
        CANCELLED
    }
}
