using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model
{
    public class UserInfor
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IpAddress { get; set; }
    }
}
