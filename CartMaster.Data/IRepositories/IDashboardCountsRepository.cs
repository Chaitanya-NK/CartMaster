using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IDashboardCountsRepository
    {
        Task<DashboardDataModel> GetDashboardDataAsync();
    }
}
