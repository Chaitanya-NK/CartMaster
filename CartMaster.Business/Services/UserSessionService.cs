using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IUserSessionRepository _userSessionRepository;
        public UserSessionService(IUserSessionRepository userSessionRepository)
        {
            _userSessionRepository = userSessionRepository;
        }
        public string EndSession(Guid sessionID)
        {
            var logoutTime = DateTime.Now;
            return _userSessionRepository.UpdateLogoutTime(sessionID, logoutTime);
        }

        public Guid StartSession(int userID)
        {
            var sessionId = Guid.NewGuid();
            var loginTime = DateTime.Now;
            _userSessionRepository.InsertUserSession(sessionId, userID, loginTime);
            return sessionId;
        }

        public void LogUserSessionChange(int userId, Guid sessionId, string ipAddress)
        {
            var sessionTracking = new UserSessionTracking
            {
                UserID = userId,
                SessionID = sessionId,
                IpAddress = ipAddress,
                ChangeDate = DateTime.Now
            };

            _userSessionRepository.InsertUserSessionTracking(sessionTracking);
        }

        public List<UserSessionTracking> GetUserSessionTracking(int userId)
        {
            return _userSessionRepository.GetUserSessionTrackingByUserId(userId);
        }
    }
}
