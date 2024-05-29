using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IConfigurationService
    {
        Task<AppActionResult> GetAllConfiguration(int pageNumber, int pageSize);
        Task<AppActionResult> UpdateConfiguration(Guid configurationId);
    }
}
