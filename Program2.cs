using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using XLDownload;

/*
 CreatedBy: Jackie Lee
 CreatedOn: 2017-10-13
*/
namespace XLDownloader
{
    class Program2
    {
        static void Main1(string[] args)
        {
            new DDD().Download();

            Console.Read();
        }


    }

    class DDD
    {
        Timer timer1;

        string target = Environment.CurrentDirectory + "\\Download\\";
        private IntPtr a;
        XL.DownTaskInfo info = new XL.DownTaskInfo();

        public DDD()
        {
            timer1 = new Timer();
            timer1.Elapsed += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var qq = XL.XL_QueryTaskInfoEx(a, info);
            Console.WriteLine("下载进度：" + (int)(info.fPercent * 100) + "%");
            //toolStripStatusLabel1.Text += string.Format("，速度{0}", (info.nSpeed / 1024.0 / 1024.0 * resultListView.CheckedItems.Count).ToString("F2") + "MB/s");     //nSpeed只能获取单个文件的下载速度，所以乘以文件数量近似计算出总速度

            if (info.stat == XL.DOWN_TASK_STATUS.TSC_COMPLETE)
            {
                Console.WriteLine("下载进度：" + "下载成功！");
                timer1.Enabled = false;
            }
        }

        public void Download()
        {
            try
            {
                timer1.Enabled = true;
                timer1.Interval = 500;
                var initSuccess = XL.XL_Init();

                if (initSuccess)
                {
                    XL.DownTaskParam p = new XL.DownTaskParam()
                    {
                        IsResume = 0,
                        //szTaskUrl = Result[listBox1.SelectedIndex],//下载地址
                        szTaskUrl = "https://codeload.github.com/Gsangu/KugouDownloader/zip/master",
                        szFilename = "KugouDownloader.zip",//保存文件名
                        szSavePath = target //下载目录
                    };
                    a = XL.XL_CreateTask(p);
                    var startSuccess = XL.XL_StartTask(a);
                }
                else
                {
                    Console.WriteLine("XL_Init初始化失败");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
