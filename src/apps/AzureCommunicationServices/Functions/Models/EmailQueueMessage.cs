using Azure.Communication.Email.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Functions.Models
{    
    public class EmailQueueMessage
    {
        [Required]
        public string[] To { get; set; }

        [Required]
        public string Subject { get; set; }
        
        public string? Importance { get; set; }

        [Required]
        public string Body { get; set; }
    }
}

