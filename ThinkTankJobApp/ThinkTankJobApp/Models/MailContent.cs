using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    internal class MailContent
    {
        public string TransactionId { get; set; }

        public string Content { get; set; }

        public string Subject { get; set; }

        public string ccAddress { get; set; }

        public string Email { get; set; }

        public int TypeOfMail { get; set; }
    }
}