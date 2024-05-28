using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IOptionPackageService
    {
        Task<AppActionResult> GetAllOptionPackage(int pageNumber, int pageSize);
        Task<AppActionResult> GetOptionPackageById(Guid optionPackageId);
        Task<AppActionResult> CreateOptionPackage(OptionPackageDto optionPackageDto);
        Task<AppActionResult> UpdateOptionPackage(Guid packageId, OptionPackageDto optionPackageDto);
        Task<AppActionResult> DeleteOptionPackage(Guid packageId);
    }
}
