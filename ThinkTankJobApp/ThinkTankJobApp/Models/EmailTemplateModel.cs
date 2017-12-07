using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class EmailTemplateModel
    {
        

            public int Id { get; set; }
            public string Content { get; set; }
            public string Subject { get; set; }
            public EmailTemplateType For { get; set; }
            public string SmsContent { get; set; }
        
         public enum EmailTemplateType
         {
             RegistrationActivation,
             RegistrationConfirmed
         }
    }
}