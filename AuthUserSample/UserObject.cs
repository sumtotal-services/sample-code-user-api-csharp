using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthUserSample
{
    public class UserObject
    {
        public string loginName { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public int domainId { get; set; }
        public int userTypeId { get; set; }
        public int usersecurityRoleId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int languageId { get; set; }
        public int timeZoneId { get; set; }
        public string status { get; set; }
    }
}
