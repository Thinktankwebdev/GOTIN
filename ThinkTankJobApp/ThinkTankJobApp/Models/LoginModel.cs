using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class LoginModel
    {
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string IpAddress { get; set; }

        
        public string UserId { get; set; }

        
        public string IpCountry { get; set; }
        
        public int FailedCount { get; set; }
        
        public string Type { get; set; }
    }
    
}