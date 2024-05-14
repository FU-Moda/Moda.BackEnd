using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface ICartService
    {
        Task<AppActionResult> GetCartItems(string accountId);
        Task<AppActionResult> UpdateCartDetail(string accountId, IEnumerable<CartDetailDto> cartDetailDtos, int pageNumber, int pageSize);
    }
}
