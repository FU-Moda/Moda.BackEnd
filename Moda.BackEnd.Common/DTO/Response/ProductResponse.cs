﻿using Moda.Backend.Domain.Models;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class ProductResponse
    {
        public Product Product { get; set; } = null!;
        public ProductStock ProductStock { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}