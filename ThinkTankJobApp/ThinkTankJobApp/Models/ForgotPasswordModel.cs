using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class ForgotPasswordModel
    {
        public string username { get; set; }
        public string deviceId { get; set; }
    }
}