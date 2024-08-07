﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class AppActionResult
    {
        public object? Result { get; set; }

        public bool IsSuccess { get; set; } = true;
        public List<string?> Messages { get; set; } = new();
    }
}
