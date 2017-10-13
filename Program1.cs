using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using XLDownload;

/*
 CreatedBy: Jackie Lee
 CreatedOn: 2017-10-13
*/
namespace XLDownloader2
{
    class Program1
    {
        static void Main2(string[] args)
        {
            if (!Directory.Exists("files"))
                Directory.CreateDirectory("files");
            new Downloader(new Parameter { SaveTo = "files" }, Console.WriteLine);

            Console.Read();
        }
    }

    class Downloader
    {
        private IntPtr ptrDownloadTask;
        //private Timer _timer;
        Timer timer;
        private Action<string> Trace { get; }

        public bool Success { get; private set; }
        public bool Finished { get; private set; }

        public Downloader(Parameter parameter, Action<string> trace)
        {
            Trace = trace;
            //XL.DownTaskInfo taskInfo = new XL.DownTaskInfo();
            //_timer = new Timer(new TimerCallback(state =>
            //{
            //    var qtInfo = XL.XL_QueryTaskInfoEx(ptrDownloadTask, _taskInfo);
            //    Trace?.Invoke($"下载进度：{(int)(taskInfo.fPercent * 100)}%，速度：{(taskInfo.nSpeed / 1024.0 / 1024.0).ToString("F2")}MB/s");
            //    if (taskInfo.stat == XL.DOWN_TASK_STATUS.TSC_COMPLETE)
            //    {
            //        Trace?.Invoke("下载进度：下载成功！");
            //        _timer.Dispose();
            //        _timer = null;
            //        Success = true;
            //        Finished = true;
            //    }
            //}), null, 0, 500);
            timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += timer1_Tick;

            Download(parameter);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            XL.DownTaskInfo taskInfo = new XL.DownTaskInfo();
            var qq = XL.XL_QueryTaskInfoEx(ptrDownloadTask, taskInfo);
            Trace?.Invoke("下载进度：" + (int)(taskInfo.fPercent * 100) + "%");

            if (taskInfo.stat == XL.DOWN_TASK_STATUS.TSC_COMPLETE)
            {
                Trace?.Invoke("下载进度：" + "下载成功！");
                timer.Enabled = false;
            }
        }

        private void Download(Parameter parameter)
        {
            try
            {
                if (!XL.XL_Init())
                {
                    Trace?.Invoke("XL_Init初始化失败");
                    return;
                }
                timer.Start();
                XL.DownTaskParam param = new XL.DownTaskParam
                {
                    IsResume = 0,
                    //szTaskUrl = parameter.Url,
                    szTaskUrl = "http://fs.vip.pc.kugou.com/201710131333/37a71efa9b14f17114a2d517d8f85188/G075/M07/0A/03/iw0DAFffseSARTOUASifkQnHJbE89.flac",
                    szFilename = "邝美云 - 去.flac",
                    szSavePath = parameter.SaveTo
                };
                ptrDownloadTask = XL.XL_CreateTask(param);
                var status = XL.XL_StartTask(ptrDownloadTask);
            }
            catch (Exception e)
            {
                Trace?.Invoke(e.Message);
                Success = false;
                Finished = true;
            }
        }
    }

    class Parameter
    {
        public string Url { get; set; }
        public string SaveTo { get; set; }
        public bool IsExit { get; set; }
    }

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
