using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace TestHack
{
    class Program
    {
        [STAThread]

        //importing WinAPI functions
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr wHnd, IntPtr childAfter, string windowName, string className);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr wHnd, char[] text, int count);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool IsWindowVisible(IntPtr wHnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ShowWindow(IntPtr wHnd, int cmd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]


        public static extern bool SendMessage(IntPtr wHnd, int msg, int wParam, int lParam);
        static private IntPtr copyWindow;
        public static bool prevCopying = false, copying = false;

        private static MyCopyWindow myWindow;
        static void Main(string[] args)
        {
            while (copyWindow == (IntPtr)0)
            {
                copyWindow = FindWindow("OperationStatusWindow", null);
                Thread.Sleep(1000);
            }


            while (true)
            {
                prevCopying = copying;
                copying = IsCopying();

                if (copying && !prevCopying)
                {
                    StartedCopying();
                }
                else if (!copying && prevCopying)
                {
                    StoppedCopying();
                }
                else if (copying)
                {
                    if (!(myWindow is null))
                    {
                        myWindow.UpdateText(GetCopyWindowText());
                    }
                }

                Thread.Sleep(33);
            }
        }


        static void StoppedCopying()
        {
            myWindow.Stop();
        }
        static void StartedCopying()
        {
            HideCopyWindow();
            Thread t = new Thread(() =>
            {
                myWindow = new MyCopyWindow();
                myWindow.ShowDialog();
            });
            t.Start();
        }

        public static bool IsCopying()
        {
            return IsWindowVisible(copyWindow);
        }
        public static void HideCopyWindow()
        {
            ShowWindow(copyWindow, 6);
        }
        public static string GetCopyWindowText()
        {
            char[] str = new char[64];
            int len = GetWindowText(copyWindow, str, 64);
            return new string(str);
        }
        public static void PauseCopy()
        {
            IntPtr control = FindWindowEx(copyWindow, (IntPtr)0, "DIRECTUIHWND", null);
            Console.WriteLine(control);
            ShowWindow(copyWindow, 3);
            SendMessage(control, 0x0100, 0x20, 0x414D0001);
            SendMessage(control, 0x0101, 0x20, 0x414D0001);
            ShowWindow(copyWindow, 6);
        }

        public static void CancelCopy()
        {
            SendMessage(copyWindow, 0x0010, 0, 0);
        }
    }
}
