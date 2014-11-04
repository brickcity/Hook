using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hook.Models
{
    using System.Collections.Specialized;
using System.Net.Http;

    public class RequestHook
    {
        public Uri Url { get; set; }
        public IList<KeyValuePair<string,string>> Headers { get; set; }
        public IList<KeyValuePair<string, string>> QueryStringValues { get; set; }
        public IList<KeyValuePair<string, string>> FormValues { get; set; }

        public string RawBody { get; set; }
        public string Method { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int Bytes { get; set; }
    }
}