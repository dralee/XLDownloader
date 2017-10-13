using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using XLDownload;

/*
 CreatedBy: Jackie Lee
 CreatedOn: 2017-10-13
*/
namespace XLDownloader
{
    class Downloader
    {
        private IntPtr ptrDownloadTask;
        private Timer timer;

        private Action<string> Info { get; }
        private Action<string> Trace { get; }

        public bool Success { get; private set; }
        public bool Finished { get; private set; }

        private Action _finish;
        private bool _isFirstTime;

        public Downloader(Parameter parameter, Action<string> info, Action<string> trace, Action finish)
        {
            Info = info;
            Trace = trace;
            _finish = finish;
            _isFirstTime = true;

            timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += timer_Tick;

            Download(parameter);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            XL.DownTaskInfo taskInfo = new XL.DownTaskInfo();
            var qq = XL.XL_QueryTaskInfoEx(ptrDownloadTask, taskInfo);
            if (_isFirstTime)
            {
                Info?.Invoke("下载进度：" + (int)(taskInfo.fPercent * 100) + "%");
                _isFirstTime = false;
            }
            else
            {
                Trace?.Invoke("下载进度：" + (int)(taskInfo.fPercent * 100) + "%");
            }

            if (taskInfo.stat == XL.DOWN_TASK_STATUS.TSC_COMPLETE)
            {
                Trace?.Invoke("下载进度：" + "下载成功！");
                timer.Enabled = false;
                _finish?.Invoke();
            }
        }

        private void Download(Parameter parameter)
        {
            try
            {
                if (!XL.XL_Init())
                {
                    Info?.Invoke("XL_Init初始化失败");
                    return;
                }
                XL.DownTaskParam param = new XL.DownTaskParam
                {
                    IsResume = 0,
                    szTaskUrl = parameter.Url,
                    //szTaskUrl = "http://fs.vip.pc.kugou.com/201710131333/37a71efa9b14f17114a2d517d8f85188/G075/M07/0A/03/iw0DAFffseSARTOUASifkQnHJbE89.flac",
                    szFilename = parameter.FileName, //"邝美云 - 去.flac",
                    szSavePath = parameter.SaveTo
                };
                ptrDownloadTask = XL.XL_CreateTask(param);
                timer.Start();

                var status = XL.XL_StartTask(ptrDownloadTask);
            }
            catch (Exception e)
            {
                Info?.Invoke(e.Message);
                Success = false;
                Finished = true;
            }
        }
    }
}
