﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class JobAppDBEntities : DbContext
    {
        public JobAppDBEntities()
            : base("name=JobAppDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<JobDetail> JobDetails { get; set; }
        public virtual DbSet<tb_email_logs> tb_email_logs { get; set; }
        public virtual DbSet<tb_email_templates> tb_email_templates { get; set; }
        public virtual DbSet<tb_login_details> tb_login_details { get; set; }
        public virtual DbSet<tb_logs> tb_logs { get; set; }
        public virtual DbSet<tb_multiple_jobs> tb_multiple_jobs { get; set; }
        public virtual DbSet<tb_registered_users> tb_registered_users { get; set; }
        public virtual DbSet<tb_registered_users_registration_tokens> tb_registered_users_registration_tokens { get; set; }
        public virtual DbSet<tb_registered_users_resetpasswordtokens> tb_registered_users_resetpasswordtokens { get; set; }
        public virtual DbSet<tb_smtp_settings> tb_smtp_settings { get; set; }
    
        public virtual ObjectResult<string> proc_LoginCheckPoint(string username, string ip_address, string ip_country)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var ip_addressParameter = ip_address != null ?
                new ObjectParameter("ip_address", ip_address) :
                new ObjectParameter("ip_address", typeof(string));
    
            var ip_countryParameter = ip_country != null ?
                new ObjectParameter("ip_country", ip_country) :
                new ObjectParameter("ip_country", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("proc_LoginCheckPoint", usernameParameter, ip_addressParameter, ip_countryParameter);
        }
    
        public virtual ObjectResult<string> proc_LoginFailed(string user_name)
        {
            var user_nameParameter = user_name != null ?
                new ObjectParameter("user_name", user_name) :
                new ObjectParameter("user_name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("proc_LoginFailed", user_nameParameter);
        }
    
        public virtual ObjectResult<string> proc_registrationConfirmation(string registration_link, string used_on_ip)
        {
            var registration_linkParameter = registration_link != null ?
                new ObjectParameter("registration_link", registration_link) :
                new ObjectParameter("registration_link", typeof(string));
    
            var used_on_ipParameter = used_on_ip != null ?
                new ObjectParameter("used_on_ip", used_on_ip) :
                new ObjectParameter("used_on_ip", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("proc_registrationConfirmation", registration_linkParameter, used_on_ipParameter);
        }
    
        public virtual ObjectResult<string> proc_ResetPassword(string password_token, string password, string used_on_origin)
        {
            var password_tokenParameter = password_token != null ?
                new ObjectParameter("password_token", password_token) :
                new ObjectParameter("password_token", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            var used_on_originParameter = used_on_origin != null ?
                new ObjectParameter("used_on_origin", used_on_origin) :
                new ObjectParameter("used_on_origin", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("proc_ResetPassword", password_tokenParameter, passwordParameter, used_on_originParameter);
        }
    
        public virtual ObjectResult<string> proc_UpdatePasswordRegToken(string dbop, Nullable<long> registered_user_id, string password_token, string ip_origin, string used_on_origin)
        {
            var dbopParameter = dbop != null ?
                new ObjectParameter("dbop", dbop) :
                new ObjectParameter("dbop", typeof(string));
    
            var registered_user_idParameter = registered_user_id.HasValue ?
                new ObjectParameter("registered_user_id", registered_user_id) :
                new ObjectParameter("registered_user_id", typeof(long));
    
            var password_tokenParameter = password_token != null ?
                new ObjectParameter("password_token", password_token) :
                new ObjectParameter("password_token", typeof(string));
    
            var ip_originParameter = ip_origin != null ?
                new ObjectParameter("ip_origin", ip_origin) :
                new ObjectParameter("ip_origin", typeof(string));
    
            var used_on_originParameter = used_on_origin != null ?
                new ObjectParameter("used_on_origin", used_on_origin) :
                new ObjectParameter("used_on_origin", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("proc_UpdatePasswordRegToken", dbopParameter, registered_user_idParameter, password_tokenParameter, ip_originParameter, used_on_originParameter);
        }
    
        public virtual ObjectResult<proc_GetEmailTemplate_Result> proc_GetEmailTemplate(string type)
        {
            var typeParameter = type != null ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_GetEmailTemplate_Result>("proc_GetEmailTemplate", typeParameter);
        }
    }
}