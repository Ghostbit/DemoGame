using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Ghostbit.Framework.Core.Utils
{
    public static class QueryParams
    {
        public static NameValueCollection ParseQueryString(string queryString)
        {
            NameValueCollection nvc = new NameValueCollection();

            if(queryString.Contains("?"))
            {
                queryString = queryString.Substring(queryString.IndexOf('?') + 1);
            }

            foreach(string vp in queryString.Split('&'))
            {
                string[] singlePair = vp.Split('=');
                if(singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    nvc.Add(singlePair[0], string.Empty);
                }
            }

            return nvc;
        }
    }
}
