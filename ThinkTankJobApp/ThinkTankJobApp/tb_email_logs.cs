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
    
    public partial class tb_email_logs
    {
        public long email_log_id { get; set; }
        public string to { get; set; }
        public string cc { get; set; }
        public byte[] created { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
        public bool has_attachement { get; set; }
        public string error { get; set; }
        public Nullable<System.DateTime> log_entered_date { get; set; }
        public string user_identifier { get; set; }
        public string attachment_path { get; set; }
    }
}
