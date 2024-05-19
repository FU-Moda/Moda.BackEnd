using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class WarehouseService : GenericBackendService, IWarehouseService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Warehouse> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public WarehouseService(IServiceProvider serviceProvider,
            IMapper mapper, IRepository<Warehouse> repository, IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<AppActionResult> GetAllWarehouse(int pageNumber, int pageSize)
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
     }
 }
