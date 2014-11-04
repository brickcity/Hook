using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hook.Models
{
    public class Hook
    {
        public string Id { get; set; }
        public List<RequestHook> Requests { get; set; }

        public Hook()
        {
            Requests = new List<RequestHook>();
        }
    }
}