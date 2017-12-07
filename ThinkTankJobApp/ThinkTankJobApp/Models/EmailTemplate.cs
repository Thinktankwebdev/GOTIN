using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class EmailTemplate
    {
        public int email_template_id { get; set; }

        public string content { get; set; }

        public string subject { get; set; }

        public TimeSpan modified { get; set; }

        public string emailfor { get; set; }

        public string type { get; set; }

        public string short_name { get; set; }

        public string sms_content { get; set; }

        public int status { get; set; }
    }
}