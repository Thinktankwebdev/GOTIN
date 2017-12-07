using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class ResetPasswordModel
    {
        public string token { get; set; }
        public string password { get; set; }
        public string used_on_ip { get; set; }
    }
}