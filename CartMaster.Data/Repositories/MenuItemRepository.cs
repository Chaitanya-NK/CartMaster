using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public MenuItemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
        }
        public List<MenuItemModel> GetMenuItemsByRoleId(int roleID)
        {
            List<MenuItemModel> menuItemModels = new List<MenuItemModel>();

            using(var connection  = _connection)
            {
                using(SqlCommand sqlCommand = new SqlCommand("GetMenuItemsByRoleId", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@RoleID", roleID);

                    connection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while(sqlDataReader.Read())
                        {
                            var menuItemModel = new MenuItemModel
                            {
                                MenuItemID = (int)sqlDataReader["MenuItemID"],
                                Name = sqlDataReader["Name"].ToString(),
                                Route = sqlDataReader["Route"].ToString()
                            };
                            menuItemModels.Add(menuItemModel);
                        }
                    }
                }
            }
            return menuItemModels;
        }
    }
}
