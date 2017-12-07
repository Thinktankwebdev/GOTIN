using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class RegisteredUser
    {
        public long RegistereduserId { get; set; }
        public string Title { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Middle_name { get; set; }
        public DateTime Dob { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email_address { get; set; }
        public string Gender { get; set; }
        public string Mbile_country_code { get; set; }
        public string Mobile_number { get; set; }
        public DateTime Registred_on { get; set; }
        public string Registration_ip { get; set; }
        public bool Is_active { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public bool isEmployer { get; set; }
        public string Organization { get; set; }
        public string token { get; set; }
        public string Address { get; set; }
    }
}