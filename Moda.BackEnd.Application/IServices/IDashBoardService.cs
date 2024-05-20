using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IDashBoardService
    {
        // last 7 day, last month, last 3 month, last 6 month, last year
        // num of order, min max order value, revenue, order value/người, order count/ người
        public Task<AppActionResult> GetRevenueReport(int timePeriod, Guid? ShopId = null);
        // top 5 Tổng từng số lượng và doanh thu product bán ra, tag nỗi bật 
        public Task<AppActionResult> GetProductReport(int timePeriod, Guid? ShopId = null);
        // How to minitor new access? Tổng account của sàn, số account mua của shop
        public Task<AppActionResult> GetUserReport(int timePeriod, Guid? ShopId = null);


    }
}
