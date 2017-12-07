using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class JobDetailsModel
    {
        public long Job_details_id { get; set; }
        public string Job_title { get; set; }
        public string Job_description { get; set; }
        public string Job_type { get; set; }
        public string Job_city { get; set; }
        public string Job_organization { get; set; }
        public System.DateTime Job_date { get; set; }
        public string Job_img { get; set; }
        public Nullable<System.DateTime> Jo_date_modified { get; set; }
        public bool is_active { get; set; }
        public Nullable<System.DateTime> Job_applied_date { get; set; }
        public long Employee_id { get; set; }
        public string username { get; set; }
        public long user_id { get; set; }
        public MultipleJobsModel Applied { get; set; }

    }

    public class JobList
    {
        public List<JobDetailsModel> Jobs { get; set; }
    }
}