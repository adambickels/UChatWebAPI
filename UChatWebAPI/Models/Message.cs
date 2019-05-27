using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UChatWebAPI.Models
{
    public class Message
    {
        public DateTime SendDate { get; set; }
        public string SenderName { get; set; }
        public string Body { get; set; }
    }
}