using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hook.Models
{
    using System.Collections.Specialized;

    public static class NameValueCollectionExtensions
    {
        public static IList<KeyValuePair<string, string>> ToKeyValuePairs(this NameValueCollection nvc)
        {
            var result = new List<KeyValuePair<string, string>>(nvc.Count);
            result.AddRange(from object item in nvc select item.ToString() into n select new KeyValuePair<string, string>(n, nvc[n]));
            return result;
        }
    }
}