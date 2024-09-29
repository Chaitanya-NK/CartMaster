using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class DashboardCountsService : IDashboardCountsService
    {
        private readonly IDashboardCountsRepository _repository;
        public DashboardCountsService(IDashboardCountsRepository repository)
        {
            _repository = repository;
        }
        public Task<DashboardDataModel> GetDashboardDataAsync()
        {
            return _repository.GetDashboardDataAsync();
        }
    }
}
