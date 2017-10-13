using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 CreatedBy: Jackie Lee
 CreatedOn: 2017-10-13
*/
namespace XLDownloader
{
    static class Extensions
    {
        public static bool IgnoreCaseEquals(this string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsValidUrl(this string url)
        {
            return !IsNullOrEmpty(url) && url.IndexOf("://") > 0;
        }
    }
}
