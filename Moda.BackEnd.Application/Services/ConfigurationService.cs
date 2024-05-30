using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class ConfigurationService : GenericBackendService, IConfigurationService
    {
        private readonly IRepository<Configuration> _repository ;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        public ConfigurationService(IServiceProvider serviceProvider,
            IRepository<Configuration> repository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _repository = repository;   
            _mapper = mapper;   
            _unitOfWork = unitOfWork;   
        }

        public async Task<AppActionResult> GetAllConfiguration(int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _repository.GetAllDataByExpression(null, pageNumber, pageSize, null, false, null);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> UpdateConfiguration(ConfigurationDto configurationDto)
        {
            var result = new AppActionResult();
            try
            {
                var configurationDb = await _repository.GetByExpression(p => p!.Name == configurationDto.Name);
                if (configurationDb == null)
                {
                    result = BuildAppActionResultError(result, "Cấu hình này không tồn tại");
                }
                configurationDb!.PreValue = configurationDto.PreValue;   
                configurationDb.ActiveValue = configurationDto.ActiveValue;
                configurationDb.ActiveDate = configurationDto.ActiveDate;
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
