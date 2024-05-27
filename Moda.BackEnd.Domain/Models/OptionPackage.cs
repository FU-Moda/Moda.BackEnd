﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class OptionPackage
    {
        [Key]
        public Guid OptionPackageId { get; set; }
        public string PackageName { get; set; } = null!;
        public DateTime Duration { get; set; }   
        public string Description { get; set; }
    }
}
