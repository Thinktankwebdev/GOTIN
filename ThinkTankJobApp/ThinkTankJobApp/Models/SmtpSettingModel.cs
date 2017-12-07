using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class SmtpSettingModel
    {
        public short SMTPSettingId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter {0}")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{0} should be {2} characters long")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter {0}")]
        [Range(21, 9999)]
        public int Port { get; set; }

        [StringLength(100, MinimumLength = 4, ErrorMessage = "{0} should be {2} characters long")]
        public string UserName { get; set; }

        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} should be {2} characters long")]
        public string Password { get; set; }

        public bool UseAnonymous { get; set; }

        public bool IsActive { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter {0}")]
        [StringLength(300, MinimumLength = 4, ErrorMessage = "{0} should be {2} characters long")]
        public string FromAddress { get; set; }
        public int Type { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}