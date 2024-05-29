using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
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

        public Task<AppActionResult> UpdateConfiguration()
        {
            var result = new AppActionResult();
            try
            {

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
        }
    }
}
