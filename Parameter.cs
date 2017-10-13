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
    class Parameter
    {
        public string Url { get; set; }
        public string SaveTo { get; set; }
        public string FileName { get; set; }
        public bool IsExit { get; set; }
        public bool IsClear { get; set; }
    }
}
