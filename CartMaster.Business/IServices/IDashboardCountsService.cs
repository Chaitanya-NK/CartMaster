using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface IDashboardCountsService
    {
        Task<DashboardDataModel> GetDashboardDataAsync();
    }
}
