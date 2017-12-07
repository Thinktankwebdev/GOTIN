using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class Response
    {
        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public bool IsValid { get; set; }
    }
}