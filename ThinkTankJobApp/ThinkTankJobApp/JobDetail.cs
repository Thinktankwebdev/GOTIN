//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThinkTankJobApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobDetail
    {
        public long job_details_id { get; set; }
        public string job_title { get; set; }
        public string job_description { get; set; }
        public string job_type { get; set; }
        public string job_city { get; set; }
        public string job_organization { get; set; }
        public System.DateTime job_date { get; set; }
        public string job_img { get; set; }
        public Nullable<System.DateTime> jo_date_modified { get; set; }
        public bool is_active { get; set; }
        public Nullable<System.DateTime> job_applied_date { get; set; }
        public long employee_id { get; set; }
    }
}