using Moda.BackEnd.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class ProductFilter
    {
        public ClothType ClothType { get; set; }    
        public Gender Gender { get; set; }      
    }
}
