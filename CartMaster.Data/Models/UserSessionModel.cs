using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Models
{
    public class UserSessionModel
    {
        public int LoginSessionID { get; set; }
        public Guid SessionID { get; set; }
        public int UserID { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserSessionTracking
    {
        public int UserSessionTrackingID { get; set; }
        public int UserID { get; set; }
        public Guid SessionID { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
