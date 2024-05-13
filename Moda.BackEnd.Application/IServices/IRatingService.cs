using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IRatingService
    {
        public Task<AppActionResult> CreateRating(CreateRatingRequest dto);
        public Task<AppActionResult> UpdateRating(UpdateRatingDto dto);
        public Task<AppActionResult> DeleteRating(Guid Id);

    }
}
