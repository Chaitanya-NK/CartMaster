using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IUserSessionRepository
    {
        string InsertUserSession(Guid sessionID, int userID, DateTime loginTime);
        string UpdateLogoutTime(Guid sessionID, DateTime logoutTime);
        void InsertUserSessionTracking(UserSessionTracking sessionTracking);
        List<UserSessionTracking> GetUserSessionTrackingByUserId(int userId);
    }
}
