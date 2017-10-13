using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

/*
 CreatedBy: Jackie Lee
 CreatedOn: 2017-10-13
*/
namespace XLDownloader
{
    class Program3
    {
        static void Main1(string[] args)
        {
            //Console.WriteLine(123456);
            //ResetLine("888");
            //ResetLine("123456");
            //ResetLine("111");
            //ResetLine("99999999999999");
            //ResetLine("321");
            TestRestLine();
            Console.Read();

            int res = SimpleCalc.Sum(8, 10);
            Console.WriteLine(res);
            res = SimpleCalc.Minus(12, 10);
            Console.WriteLine(res);

            Console.Read();
        }

        static void TestRestLine()
        {
            string[] items = {
                "888","12343289102341234231","12348912342314","8888888888888","222222","333333333333","1111","55555555","666"
            };

            Timer timer = new Timer();
            timer.Interval = 500;
            int i = 0;
            timer.Elapsed += (sender, e) =>
            {
                if (items.Length > i)
                {
                    ResetLine(items[i]);
                    ++i;
                }
                else
                {
                    timer.Stop();
                    Console.WriteLine("End");
                }
            };
            timer.Start();
        }

        static void ResetLine(string msg)
        {
            Console.SetCursorPosition(0, Console.CursorTop > 0 ? Console.CursorTop - 1 : 0);
            Console.WriteLine(GetEmptyLine());
            Console.SetCursorPosition(0, Console.CursorTop - 2);
            Console.WriteLine(msg);
        }

        private static string _emptyLine;
        private static int _lastWidth;

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
    }

    static class SimpleCalc
    {
        [DllImport("SimipleCalcLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Sum(int x, int y);

        [DllImport("SimipleCalcLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Minus(int x, int y);
    }
}
