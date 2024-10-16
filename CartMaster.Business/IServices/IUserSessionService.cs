using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface IUserSessionService
    {
        Guid StartSession(int userID);
        string EndSession(Guid sessionID);
        void LogUserSessionChange(int userId, Guid sessionId, string ipAddress);
        List<UserSessionTracking> GetUserSessionTracking(int userId);
    }
}
