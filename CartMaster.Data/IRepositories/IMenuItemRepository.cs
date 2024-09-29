using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IMenuItemRepository
    {
        public List<MenuItemModel> GetMenuItemsByRoleId(int roleID);
    }
}
