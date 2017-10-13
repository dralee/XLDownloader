using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using XLDownload;
/*
测试结论：.net core加载c++类库，只能加载平台为x64的，而不能加载x86的
XL中类库为x86的，故会加载失败
*/

/*
 CreatedBy: Jackie Lee
 CreatedOn: 2017-10-13
*/
namespace XLDownloader
{
    class Program
    {
        private const string KEY_URL = "url";
        private const string KEY_SAVETO = "saveto";
        private const string KEY_FILENAME = "filename";
        private const string KEY_CLEAR = "clear";
        private const string KEY_EXIT = "exit";

        private static string _emptyLine;
        private static int _lastWidth;

        static Action<string> Output = str => Console.WriteLine(str);
        static Action<string, string> OutputItem = (str, str2) => Console.Write(str, str2);
        static Action ShowMenu = () =>
         {
             Output("******************************************************");
             Output("参数：");
             OutputItem("{0,12}", "url:"); Output(" 下载的文件url路径");
             OutputItem("{0,12}", "saveTo:"); Output(" 下载文件保存路径（默认为当前目录下的：files目录）");
             OutputItem("{0,12}", "filename:"); Output(" 文件保存名称");
             OutputItem("{0,12}", "clear:"); Output(" 清屏");
             Output("示例：");
             Output("  url=http://baidu.com/xxxx.jpg filename=xxx.jpg saveTo=D:/download 即可将Url指向的jpg文件下载到D盘中download目录");
             Output("******************************************************");
             Output("请输入参数（输入exit退出程序）：");
         };

        static void Main(string[] args)
        {
            Menu:
            ShowMenu();
            var input = Console.ReadLine();
            if (!Resove(input, out Parameter parameter))
            {
                if (parameter.IsClear)
                    Console.Clear();
                if (!parameter.IsExit)
                {
                    Output("参数输入不正确，url与filename为必须项");
                    goto Menu;
                }
                Output("Byte!");
                return;
            }

            if (parameter.SaveTo.IsNullOrEmpty())
            {
                parameter.SaveTo = "files";
            }

            if (!Directory.Exists(parameter.SaveTo))
            {
                Directory.CreateDirectory(parameter.SaveTo);
            }

            bool needAgain = false;
            bool finish = false;
            Downloader downloader = new Downloader(parameter, Output, ResetLine, () =>
            {
                Output("是否继续下载操作？(y/n)");
                var q = Console.ReadLine();
                if (q.IgnoreCaseEquals("y") || q.IgnoreCaseEquals("yes"))
                {
                    needAgain = true;
                }
                finish = true;
            });

            while (!finish) ;

            if (needAgain)
                goto Menu;

            Output("Byte!");
        }

        static void ResetLine(string msg)
        {
            Console.SetCursorPosition(0, Console.CursorTop > 0 ? Console.CursorTop - 1 : 0);
            Console.WriteLine(GetEmptyLine());
            Console.SetCursorPosition(0, Console.CursorTop - 2);
            Console.WriteLine(msg);
        }

        static string GetEmptyLine()
        {
            if (_lastWidth != Console.WindowWidth)
            {
                _lastWidth = Console.WindowWidth;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < _lastWidth; ++i)
                {
                    sb.Append(" ");
                }
                _emptyLine = sb.ToString();
            }
            return _emptyLine;
        }

        static bool Resove(string args, out Parameter parameter)
        {
            parameter = new Parameter();
            if (string.IsNullOrWhiteSpace(args))
            {
                Output("参数不参为空");
                return false;
            }

            var keys = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var k in keys)
            {
                if (k.IgnoreCaseEquals(KEY_EXIT))
                {
                    parameter.IsExit = true;
                    return false;
                }
                if (k.IgnoreCaseEquals(KEY_CLEAR))
                {
                    parameter.IsClear = true;
                    return false;
                }
                var items = k.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length != 2)
                {
                    Output($"参数{k}输入有误");
                    return false;
                }
                if (items[0].IgnoreCaseEquals(KEY_URL))
                {
                    parameter.Url = items[1];
                }
                else if (items[0].IgnoreCaseEquals(KEY_SAVETO))
                {
                    parameter.SaveTo = items[1];
                }
                else if (items[0].IgnoreCaseEquals(KEY_FILENAME))
                {
                    parameter.FileName = items[1];
                }
            }
            return parameter.Url.IsValidUrl() && !parameter.FileName.IsNullOrEmpty();
        }
    }
}
