using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class MultipleJobsModel
    {
        public long Multiple_jobs_id { get; set; }
        public long Registered_user_id { get; set; }
        public long Employee_id { get; set; }
        public long Jobs_details_id { get; set; }
        public DateTime Applied_date { get; set; }
    }
}